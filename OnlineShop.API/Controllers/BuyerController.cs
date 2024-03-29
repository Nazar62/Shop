using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OnlineShop.API.Models.DTO;
using OnlineShop.API.Models.Requests;
using OnlineShop.API.Repo.Interfaces;
using System.Net;

namespace OnlineShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyerController : Controller
    {
        private readonly IBuyer _buyerRepo;
        private readonly IEmail _emailRepo;
        public BuyerController(IBuyer buyerRepo, IEmail emailRepo)
        {
            _buyerRepo = buyerRepo;
            _emailRepo = emailRepo;
        }

        [HttpPost]
        [Route("/SignUp")]
        public IActionResult SignUp(BuyerDTO buyerDto)
        {
            if (buyerDto == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_buyerRepo.UserExistsEmail(buyerDto.Email))
            {
                return BadRequest(Json("Email alredy used"));
            }
            if(_buyerRepo.UserExists(buyerDto.Name))
            {
                return BadRequest(Json("Login alredy used"));
            }
            if(!_buyerRepo.CreateBuyer(buyerDto))
            {
                ModelState.AddModelError("value", "Something went wrong creating");
                return BadRequest(ModelState);
            }
            var buyer = _buyerRepo.GetBuyer(buyerDto.Name);
            Verify(buyer.Name);
            return Ok(buyer);
        }


        [HttpPost]
        [Route("/LogIn")]
        public IActionResult LogIn(BuyerDTO buyerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (buyerDto == null)
            {
                return BadRequest();
            }
            if(!_buyerRepo.UserExists(buyerDto.Name))
            {
                return BadRequest(Json("User not exists"));
            }
            var buyer = _buyerRepo.GetBuyer(buyerDto.Name);
            var hashedPass = _buyerRepo.HashPassword(buyerDto.Password);
            if(buyer.Password != hashedPass)
            {
                return BadRequest(Json("Password incorrect"));
            }
            if (buyer.Verified == false)
            {
                return BadRequest(Json("Not verified check email"));
            }
            return Ok(buyer);
        }

        [HttpGet("verify/{token}")]
        //[Route("verify/{token}")]
        public ContentResult VerifyHtml(Guid token)
        {
            var user = _buyerRepo.GetBuyerByVerifyToken(token);
            var res = "Verified you can close this page";
            user.Verified = true;
            if (!_buyerRepo.UpdateBuyer(user))
            {
                res = "Something went wrong updating";
            }
            var html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Verify</title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Montserrat:100,200,300,regular,500,600,700,800,900,100italic,200italic,300italic,italic,500italic,600italic,700italic,800italic,900italic\" rel=\"stylesheet\" />\r\n</head>\r\n<body style=\"display: flex; align-items: center; justify-content: center; background-color: #424242;\">\r\n    <p style=\"color: white; font-family: Montserrat;\">{res}</p>\r\n</body>\r\n</html>";
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }

        private void Verify(string name) 
        {
            //TODO: Create Verify By Email
            var user = _buyerRepo.GetBuyer(name);
            _buyerRepo.CreateVerificationToken(name);
            var baseUrl = $"https://{Request.Host.Value}/api/Buyer/verify/{user.VerificationToken}";
            _emailRepo.SendMail($"<h1>Verify Account Notes</h1> <div>{baseUrl}</div>", user.Email, "Verify Account");
        }

        [HttpPost]
        [Route("ChangeAddress")]
        public IActionResult ChangeAddress(ChangeAddresRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (request == null)
            {
                return BadRequest();
            }

            var user = _buyerRepo.GetBuyer(request.BuyerGuid);

            if(user == null)
            {
                return BadRequest(Json("User note exists"));
            }

            if(!_buyerRepo.UpdateBuyer(user))
            {
                ModelState.AddModelError("value", "Something went wrong updating");
                return BadRequest(ModelState);
            }

            return Ok(Json("Updated successfully"));
        }

        [HttpDelete("DeleteBuyer/{id}")]
        public IActionResult DeleteUser(int id, [FromBody]BuyerDTO buyerDTO)
        {
            var user = _buyerRepo.GetBuyer(buyerDTO.Name);

            if (id != user.Id)
                return BadRequest(Json("Incorrect id"));

            if (!_buyerRepo.UserExists(buyerDTO.Name))
                return BadRequest(Json("User not exists"));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_buyerRepo.HashPassword(buyerDTO.Password) != user.Password)
                return BadRequest(Json("Incorrect password"));
            try
            {
                _buyerRepo.CreateVerificationToken(user.Name);
                var baseUrl = $"https://{Request.Host.Value}/api/Buyer/verify/delete/{user.VerificationToken}";
                _emailRepo.SendMail($"<h1>Verify Account Delete</h1> <div>{baseUrl}</div>", user.Email, "Verify Account Delete");
                return Ok();
            } catch
            {
                return BadRequest(Json("Something went wrong deleting"));
            }
        }

        [HttpGet("verify/delete/{token}")]
        //[Route("verify/{token}")]
        public ContentResult VerifyDeleteHtml(Guid token)
        {
            var user = _buyerRepo.GetBuyerByVerifyToken(token);
            var res = "Verified account deleted";

            if (user.TokenExpires < DateTime.Now)
                res = "Token Expires";

            user.Verified = true;
            if (!_buyerRepo.DeleteUser(user))
            {
                res = "Something went wrong deleting";
            }
            var html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Verify</title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Montserrat:100,200,300,regular,500,600,700,800,900,100italic,200italic,300italic,italic,500italic,600italic,700italic,800italic,900italic\" rel=\"stylesheet\" />\r\n</head>\r\n<body style=\"display: flex; align-items: center; justify-content: center; background-color: #424242;\">\r\n    <p style=\"color: white; font-family: Montserrat;\">{res}</p>\r\n</body>\r\n</html>";
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }

        [HttpPost]
        [Route("forgot-password")]
        public IActionResult ForgotPassword(BuyerDTO buyerDto)
        {
            var user = _buyerRepo.GetBuyer(buyerDto.Name);
            if (user == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(!_buyerRepo.CreateVerificationToken(user.Name))
            {
                ModelState.AddModelError("value", "Something went wrong updating");
                return BadRequest(ModelState);
            }
            var baseUrl = $"https://{Request.Host.Value}/api/Buyer/reset-password-verify/{user.VerificationToken}";
            _emailRepo.SendMail($"<h1>Reset password Notes</h1> <div>{baseUrl}</div>", user.Email, "Confirm reset password");
            return Ok();
        }

        [HttpGet("reset-password-verify/{token}")]
        public ContentResult VerifyResetPassword(Guid token)
        {
            var user = _buyerRepo.GetBuyerByVerifyToken(token);
            var res = "Verified";

            if (user.TokenExpires < DateTime.Now)
                res = "Token Expires";

            if (!_buyerRepo.VerifyToken(user.Name))
            {
                res = "Something went wrong deleting";
            }
            _buyerRepo.VerifyToken(user.Name);
            var html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Verify</title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Montserrat:100,200,300,regular,500,600,700,800,900,100italic,200italic,300italic,italic,500italic,600italic,700italic,800italic,900italic\" rel=\"stylesheet\" />\r\n</head>\r\n<body style=\"display: flex; align-items: center; justify-content: center; background-color: #424242;\">\r\n    <p style=\"color: white; font-family: Montserrat;\">{res}</p>\r\n</body>\r\n</html>";
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }

        [HttpPost]
        [Route("reset-password")]
        public IActionResult ResetPassword(ResetPasswordRequest request)
        {
            var user = _buyerRepo.GetBuyerByVerifyToken(request.Token);
            if (user == null)
                return BadRequest(Json("Invalid token"));
            if (!_buyerRepo.UserExists(user.Name))
            {
                ModelState.AddModelError("value", "User not exists");
                return BadRequest(ModelState);
            }
            if (user.TokenExpires < DateTime.Now)
                return BadRequest(Json("Token expired"));
            if (user.TokenVerified == false)
                return BadRequest(Json("Reset not confirmed check email"));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var hash = _buyerRepo.HashPassword(request.Password);
            user.Password = hash;
            _buyerRepo.CloseVerificationToken(user.Name);

            if (!_buyerRepo.UpdateBuyer(user))
            {
                ModelState.AddModelError("value", "Something went wrong updating");
                return BadRequest(ModelState);
            }
            return Ok(Json("Password successfully reset"));
        }

        [HttpPost]
        [Route("change-email")]
        public IActionResult ChangeEmail(ChangeEmailRequest request)
        {
            var user = _buyerRepo.GetBuyer(request.Id);
            if (user == null)
                return BadRequest(Json("Token not used"));
            _buyerRepo.CreateVerificationToken(user.Name);
            var res = "";
            if (_buyerRepo.UserExistsEmail(request.NewEmail))
                return BadRequest(Json("Email is alredy used"));

            if (!_buyerRepo.UpdateBuyer(user))
            {
                return StatusCode(500, Json("Something went wrong updating"));
            }
            var baseUrl = $"https://{Request.Host.Value}/api/Buyer/verify-old/{user.VerificationToken}/{request.NewEmail}";
            _emailRepo.SendMail($"<h1>Change Email Notes</h1> <div>{baseUrl}</div>", user.Email, "Confirm change Email");
            return Ok(Json("Check email"));
        }

        [HttpGet("verify-old/{token}/{newEmail}")]
        public ContentResult VerifyChangeEmailFromOldEmail(Guid token, string newEmail)
        {
            var user = _buyerRepo.GetBuyerByVerifyToken(token);
            var res = "Verified! Check new email!";
            user.TokenVerified = true;
            _buyerRepo.UpdateBuyer(user);
            if (!_buyerRepo.UpdateBuyer(user))
            {
                res = "Something went wrong updating";
            }
            var html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Verify Email Changing</title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Montserrat:100,200,300,regular,500,600,700,800,900,100italic,200italic,300italic,italic,500italic,600italic,700italic,800italic,900italic\" rel=\"stylesheet\" />\r\n</head>\r\n<body style=\"display: flex; align-items: center; justify-content: center; background-color: #424242;\">\r\n    <p style=\"color: white; font-family: Montserrat;\">{res}</p>\r\n</body>\r\n</html>";
            var baseUrl = $"https://{Request.Host.Value}/api/User/verify-new/{user.VerificationToken}/{newEmail}";
            _emailRepo.SendMail($"<h1>Change Email Notes</h1> <div>{baseUrl}</div>", newEmail, "Confirm change email");
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }

        [HttpGet("verify-new/{token}/{email}")]
        public ContentResult VerifyChangeEmailFromNewEmail(Guid token, string email)
        {
            var res = "Verified! Email changed!";
            var user = _buyerRepo.GetBuyerByVerifyToken(token);
            if (user == null)
            {
                res = "Url expired";
                var htmls = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Verify Email Changing</title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Montserrat:100,200,300,regular,500,600,700,800,900,100italic,200italic,300italic,italic,500italic,600italic,700italic,800italic,900italic\" rel=\"stylesheet\" />\r\n</head>\r\n<body style=\"display: flex; align-items: center; justify-content: center; background-color: #424242;\">\r\n    <p style=\"color: white; font-family: Montserrat;\">{res}</p>\r\n</body>\r\n</html>";
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = htmls
                };
            }

            try
            {
                user.TokenExpires = DateTime.Now;
                user.TokenVerified = false;
                user.Email = email;
                if(_buyerRepo.UpdateBuyer(user))
                {
                    res = "Something went wrong updating";
                }
            }
            catch
            {
            }
            
            var html = $"<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Verify Email Changing</title>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Montserrat:100,200,300,regular,500,600,700,800,900,100italic,200italic,300italic,italic,500italic,600italic,700italic,800italic,900italic\" rel=\"stylesheet\" />\r\n</head>\r\n<body style=\"display: flex; align-items: center; justify-content: center; background-color: #424242;\">\r\n    <p style=\"color: white; font-family: Montserrat;\">{res}</p>\r\n</body>\r\n</html>";
            return new ContentResult
            {
                ContentType = "text/html",
                StatusCode = (int)HttpStatusCode.OK,
                Content = html
            };
        }

        [HttpPost]
        [Route("buy-product")]
        public IActionResult BuyProduct()
        {

        }
    }
}
