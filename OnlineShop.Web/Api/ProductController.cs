using AutoMapper;
using OnlineShop.Model.Models;
using OnlineShop.Service;
using OnlineShop.Web.infrastructure.Core;
using OnlineShop.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineShop.Web.infrastructure.Extensions;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web.Http.Cors;

namespace OnlineShop.Web.Api
{
  
    [RoutePrefix("api/product")]
    
    public class ProductController : ApiControllerBase
    {
        #region Initialize
        private IProductService _productService;

        public ProductController(IErrorService errorService , IProductService productService) : base(errorService)
        {
            this._productService = productService;
        }

        #endregion

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetAll();

                var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;

            });
        }

     
        [Route("getbyid/{id:int}")]
        [HttpGet]

        public HttpResponseMessage GetById(HttpRequestMessage request , int id)
        {
            return CreateHttpResponse(request, () =>
           {
               var model = _productService.GetById(id);

               var responseData = Mapper.Map<Product, ProductViewModel>(model);

               var response = request.CreateResponse(HttpStatusCode.OK, responseData);

               return response;
           });
        }

        [Route("create")]
        [HttpPost]
        
        [AllowAnonymous]

        public HttpResponseMessage create()
        {

            var newProduct = new Product();

            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload Image

            var postedFile = httpRequest.Files["Image"];

            var val = httpRequest["Value"];

            ProductViewModel productVm = JsonConvert.DeserializeObject<ProductViewModel>(val);//convert json to Object

            //create custom filename
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);
            postedFile.SaveAs(filePath);

            //Save to DB

            productVm.Image = imageName;
            newProduct.UpdateProduct(productVm);
            newProduct.CreatedDate = DateTime.Now;
            _productService.Add(newProduct);
            _productService.Save();

            var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);

            return Request.CreateResponse(HttpStatusCode.Created, responseData);

        }

        /*public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productVm)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               if (!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);

               }
               else
               {
                   var newProduct = new Product();

                  *//* string imageName = null;
                   var httpRequest = HttpContext.Current.Request;
                   //Upload Image

                   var postedFile = httpRequest.Files["Image"];
                   //productVm = httpRequest["Value"];
                   //create custom filename
                   imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                   imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                   var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);
                   postedFile.SaveAs(filePath);

                   newProduct.Image = imageName;*//*

                   newProduct.UpdateProduct(productVm);
                   newProduct.CreatedDate = DateTime.Now;
                   _productService.Add(newProduct);
                   _productService.Save();

                   var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);

               }

               return response;
           });
        }*/

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
           {
                HttpResponseMessage response = null;
                var url = HttpContext.Current.Request.Url;

               if (!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest , ModelState);
               }
               else
               {
                   var newProduct = new Product();
                   var httpRequest = HttpContext.Current.Request;
                   //Upload Image
                   var postedFile = httpRequest.Files["Image"];
                   var val = httpRequest["Value"];

                   ProductViewModel productVm = JsonConvert.DeserializeObject<ProductViewModel>(val);//convert json to Object

                   var dbProduct = _productService.GetById(productVm.ID);//product cũ

                   dbProduct.UpdateProduct(productVm);

                   dbProduct.UpdatedDate = DateTime.Now;

                   //Lấy tên hình ảnh

                   var listString = dbProduct.Image.Split('/');

                   var imageName = listString[listString.Length - 1];
                   var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);

                   if(File.Exists(filePath))
                   {
                       dbProduct.Image = imageName;
                   }
                   else
                   {
                       //Xóa bỏ image cũ
                       File.Delete(filePath);
                       //Add Image mới vào
                       //create custom filename
                       imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
                       imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
                       filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + imageName);

                       dbProduct.Image = imageName;
                       postedFile.SaveAs(filePath);
                   }

                   _productService.Update(dbProduct);
                   _productService.Save();

                   var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);
               }
               return response;
           });
        }
        
        [Route("delete/{id}")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request , int id)
        {
            return CreateHttpResponse(request, () =>
           {
               HttpResponseMessage response = null;
               if (!ModelState.IsValid)
               {
                   response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
               }
               else
               {
                   var oldProduct = _productService.Delete(id);
                   
                   var filePath = HttpContext.Current.Server.MapPath("~/Assets/Images/" + oldProduct.Image);
                   if (File.Exists(filePath))
                   {
                       File.Delete(filePath);
                   }

                   _productService.Save();
                   var responseData = Mapper.Map<Product, ProductViewModel>(oldProduct);
                   response = request.CreateResponse(HttpStatusCode.Created, responseData);
               }

               return response;
           });
        }

        [HttpGet]
        [Route("GetImage")]
        //Download image file api
        public HttpResponseMessage GetImage(HttpRequestMessage request)
        {
            //Create HTTP Response

            /*HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            //Set the File Path
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Image/") + image + ".PNG";
            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.  
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", image);
                throw new HttpResponseException(response);
            }
            //read the File into a Byte Array
            byte[] bytes = File.ReadAllBytes(filePath);
            //Set the Response Content.  
            response.Content = new ByteArrayContent(bytes);
            //Set the Response Content Length.  
            response.Content.Headers.ContentLength = bytes.LongLength;
            //Set the Content Disposition Header Value and FileName.  
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = image + ".PNG";
            //Set the File Content Type.  
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(image + ".PNG"));
            return response;*/

            return CreateHttpResponse(request, () =>
            {
                var url = HttpContext.Current.Request.Url;

                var responseData = url.Scheme + "://" + url.Host + ":" + url.Port + "/Assets/Images/";

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;

            });
        }





    }
}
