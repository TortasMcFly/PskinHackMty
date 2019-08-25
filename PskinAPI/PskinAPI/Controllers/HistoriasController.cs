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
    [RoutePrefix("api/Historias")]
    public class HistoriasController : ApiController
    {
        [HttpGet]
        [Route("GetAll")]
        public IHttpActionResult Historias_GetAll()
        {
            List<Historia> historias = ADOHistorias.GetAllHistoriasDesc();

            if (historias != null || historias.Count > 0)
                return Ok(historias);
            else
                return BadRequest("No hay historias disponibles");
        }

        [HttpPost]
        [Route("Like")]
        public IHttpActionResult Like()
        {
            return Ok();
        }

        [HttpPost]
        [Route("GuardarHistoria")]
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
                                                    string Descripcion = Convert.ToString(header.GetValues("Descripcion").First());
                                                    int idUsuario = Convert.ToInt32(header.GetValues("idUsuario").First());
                                                    ADOHistorias.Insert_Historia(postedFile.FileName, Descripcion , idUsuario);
                                                    Usuario u = ADO_Usuario.GetUserById(idUsuario);
                                                    return Ok(u);
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
