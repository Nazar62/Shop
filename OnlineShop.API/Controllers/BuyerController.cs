using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Models.DTO;
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
            if(buyerDto == null)
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
                return BadRequest(Json("Note verified check email"));
            }
            return Ok(buyer);
        }

        [HttpGet("verify/{token}")]
        //[Route("verify/{token}")]
        public ContentResult VerifyHtml(string token)
        {
            var user = _buyerRepo.GetBuyer(token);
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

        private void Verify(string name) //https://localhost:7055/api/User/verify/ef6e012e-706b-411b-a8be-30506c7b82bc)
        {
            //TODO: Create Verify By Email
            var user = _buyerRepo.GetBuyer(name);
            _buyerRepo.CreateVerificationToken(name);
            var baseUrl = $"https://{Request.Host.Value}/api/User/verify/{user.VerificationToken}";
            _emailRepo.SendMail($"<h1>Verify Account Notes</h1> <div>{baseUrl}</div>", user.Email, "Verify Account");
        }
    }
}
