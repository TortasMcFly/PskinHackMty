using Newtonsoft.Json;
using PskinAPI.ADO;
using PskinAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace PskinAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Usuarios")]
    public class UsuarioController : ApiController
    {
        [HttpGet]
        [Route("Login")]
        public IHttpActionResult Login()
        {
            var headers = Request.Headers;
            Usuario usu = null;

            if (headers.Contains("Email") && headers.Contains("Pass"))
                usu = ADO_Usuario.GetUsuarioEmail(Convert.ToString(headers.GetValues("Email").First()) , Convert.ToString(headers.GetValues("Pass").First()));
            else
                usu = ADO_Usuario.GetUsuarioUsername(Convert.ToString(headers.GetValues("Username").First()) , Convert.ToString(headers.GetValues("Pass").First()));

            if (usu != null)
                return Ok(usu);
            else
                return BadRequest("No se encontro el usuario");
        }

        [HttpPost]
        [Route("Registro")]
        public IHttpActionResult Registro()
        {
            var headers = Request.Headers;
            string usuJson = Convert.ToString(headers.GetValues("Usuario").First());

            Usuario usu = JsonConvert.DeserializeObject<Usuario>(usuJson);

            int lastID = ADO_Usuario.Register(usu);

            if (lastID > 0)
            {
                usu.Id = lastID;
                return Ok(usu);
            }
            else
            {
                return BadRequest("Ocurrio un error");
            }
        }

        [HttpPost]
        [Route("Actualizar")]
        public IHttpActionResult Actualizar()
        {
            var headers = Request.Headers;
            string usuJson = Convert.ToString(headers.GetValues("Usuario").First());

            Usuario usu = JsonConvert.DeserializeObject<Usuario>(usuJson);

            Usuario sus = ADO_Usuario.Actualizar(usu);

            Usuario fin = ADO_Usuario.GetUserById(sus.Id);

            if (fin != null)
            {
                return Ok(fin);
            }
            else
            {
                return BadRequest("Ocurrio un error");
            }
        }

        [HttpPost]
        [Route("SubirImagen")]
        public IHttpActionResult GuardarImagen()
        {
            try
            {
                try
                {

                    var httpRequest = HttpContext.Current.Request;
                    foreach (string file in httpRequest.Files)
                    {
                        try
                        {
                            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                            var postedFile = httpRequest.Files[file];
                            if (postedFile != null && postedFile.ContentLength > 0)
                            {

                                try
                                {
                                    int MaxContentLength = 10240 * 10240 * 1; // 1 MB  

                                    IList<string> AllowedFileExtensions = new List<string> { ".jpg" , ".gif" , ".png" };
                                    var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                                    var extension = ext.ToLower();

                                    try
                                    {
                                        if (!AllowedFileExtensions.Contains(extension))
                                        {
                                            try
                                            {
                                                var message = string.Format("Solo se permiten extensiones .jpg,.gif,.png.");

                                                return BadRequest(message);
                                            }
                                            catch (Exception ex)
                                            {
                                                var lo = ex.ToString();
                                                return BadRequest(lo);
                                            }

                                        }
                                        else if (postedFile.ContentLength > MaxContentLength)
                                        {
                                            try
                                            {
                                                var message = string.Format("Solo archivos de hasta 1 mb.");

                                                return BadRequest(message);
                                            }
                                            catch (Exception ex)
                                            {
                                                var lo = ex.ToString();
                                                return BadRequest(lo);
                                            }

                                        }
                                        else
                                        {
                                            try
                                            {
                                                var filePath = "";
                                                try
                                                {
                                                   
                                                    filePath = HttpContext.Current.Server.MapPath("~/Imagenes/" + postedFile.FileName);
                                                    
                                                }
                                                catch (Exception ex)
                                                {
                                                    var lo = ex.ToString();
                                                    return BadRequest(lo);
                                                }
                                                try
                                                {
                                                    postedFile.SaveAs(filePath);

                                                    var header = Request.Headers;
                                                    int id = Convert.ToInt32(header.GetValues("IdUsuario").First());
                                                    ADO_Usuario.UploadImage(postedFile.FileName , id);
                                                    Usuario a = ADO_Usuario.GetUserById(id);
                                                    return Ok(a);
                                                }
                                                catch (Exception ex)
                                                {
                                                    var lo = string.Format(filePath);
                                                    return BadRequest(lo);
                                                }

                                            }
                                            catch (Exception ex)
                                            {
                                                var lo = ex.ToString();
                                                return BadRequest(lo);
                                            }

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        var lo = ex.ToString();
                                        return BadRequest(lo);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    var lo = ex.ToString();
                                    return BadRequest(lo);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            var lo = ex.ToString();
                            return BadRequest(lo);
                        }


                    }
                }

                catch (Exception ex)
                {
                    var lo = ex.ToString();
                    return BadRequest(lo);
                }


                var res = string.Format("Request sin archivo.");
                return BadRequest(res);
            }
            catch (Exception ex)
            {
                string res = ex.ToString();
                return BadRequest(res);
            }
        }
    }
}
