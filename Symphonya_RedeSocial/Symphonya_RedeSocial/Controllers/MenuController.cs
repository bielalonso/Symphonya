﻿using Symphonya_RedeSocial.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI.HtmlControls;

namespace Symphonya_RedeSocial.Controllers
{
    public class MenuController : Controller
    {

        // GET: Menu
        public ActionResult Feed()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                Int32 IDUsuario;
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                //VIEWBAG RELACIONADA AS POSTAGENS
                ViewBag.Postagens = Postagem.ListarPostagem(IDUsuario);

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    //CASO O USUARIO TENHA PESQUISADO ALGO
                    if (Request.Form["busca"] != null)
                    {
                        String busca = Request.Form["busca"].ToString();
                        Response.Redirect("/Menu/Pesquisar/" + busca);
                    }

                    if(Request.Form["TxtAreaPostagem"] != null)
                    {
                        Postagem postagem = new Postagem();
                        postagem.AutorID = IDUsuario;
                        postagem.Texto = Request.Form["TxtAreaPostagem"].ToString();
                        postagem.Data_Hora = DateTime.Now;
                        postagem.Postar();
                    }
                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }

        public ActionResult Perfil()
        {
            //CRIA VARIAVEL GLOBAL DO ID DO USUARIO 
            Int32 IDUsuario;

            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.Instrumentos = Instrumentos.ListarEspecifico(User.ID, true);
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.Instrumentos = Instrumentos.ListarEspecifico(User.ID, true);
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();
                ViewBag.Arquivos = Models.Arquivos.ListarArquivoUsuario(IDUsuario, true);

                //VIEWBAG RELACIONADA AS POSTAGENS DO USUARIO
                ViewBag.Postagens = Postagem.ListarPostagem(IDUsuario);

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }

            }

            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }
            return View();
        }

        public ActionResult CadastroBanda()
        {
            Int32 IDUsuario;
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                //RETORNA OS USUARIOS, CASO HAJA RESULTADO
                if (Genero.Listar() != null)
                {
                    List<Genero> generos = Genero.Listar();
                    ViewBag.Generos = generos;
                }

                if(Bandas.MostrarBanda(IDUsuario) != null)
                {
                    Response.Redirect("/Menu/VerBandas");
                }

                if (Request.HttpMethod == "POST")
                {
                    Bandas ba = new Bandas();
                    ba.Nome = Request.Form["Nome"].ToString();
                    ba.Descricao = Request.Form["Descricao"].ToString();
                    if (ba.NovaBanda(IDUsuario))
                    {
                        Bandas bandaAux = new Bandas();
                        bandaAux = ba.VerificarBandas(IDUsuario);
                        bandaAux.NovaBandaIntegrante(IDUsuario);

                    }
                    
                    Response.Redirect("/Menu/Feed");
                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");

            }
            return View();
        }

        public ActionResult Agenda()
        {

            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO 
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Show.Listar(IDUsuario) != null)
                {
                    List<Show> showsUsuario = Show.Listar(IDUsuario);
                    ViewBag.Shows = showsUsuario;
                }

                if (Seguidores.ListarSeguidores(IDUsuario) != null)
                {
                    List<Seguidores> seguidores = Seguidores.ListarSeguidores(IDUsuario);
                    ViewBag.Seguidores = seguidores;
                }
                else
                {
                    ViewBag.Erro = "Não foram encontrados seguidores!";
                }
                if (Request.HttpMethod == "POST")
                {

                    Show sh = new Show();
                    Agenda agenda = new Agenda();

                    if (Models.Agenda.Mostrar(IDUsuario))
                    {
                        agenda = new Agenda(IDUsuario);
                    }
                    else
                    {
                        agenda.NovaAgenda(IDUsuario);
                    }

                    if(agenda != null)
                    {
                        sh.Titulo = Request.Form["Titulo"].ToString();
                        sh.Descricao = Request.Form["Descricao"].ToString();
                        sh.Hora = Request.Form["Hora"].ToString();
                        sh.Data = Request.Form["Data"].ToString();
                        sh.UsuarioID = ViewBag.User.ID;
                        sh.AgendaID = agenda.ID;
                        sh.NovoEvento(IDUsuario, agenda.ID);
                        Response.Redirect("/Menu/Feed");
                    }
                }

            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");

            }
            return View();
        }
        public ActionResult NovoEvento()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO 
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Seguidores.ListarSeguidores(IDUsuario) != null)
                {
                    List<Seguidores> seguidores = Seguidores.ListarSeguidores(IDUsuario);
                    ViewBag.Seguidores = seguidores;
                }
                else
                {
                    ViewBag.Erro = "Não foram encontrados seguidores!";
                }
                if (Request.HttpMethod == "POST")
                {
                    Show sh = new Show();

                    sh.Titulo = Request.Form["Titulo"].ToString();
                    sh.Descricao = Request.Form["Descricao"].ToString();
                    sh.Hora = Request.Form["Hora"].ToString();
                    sh.Data = Request.Form["Data"].ToString();
                    sh.UsuarioID = ViewBag.User.ID;
                    sh.AgendaID = ViewBag.User.ID;
                    //sh.NovoEvento();

                }

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }
               
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }

        public ActionResult NovaAgenda()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO 
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Seguidores.ListarSeguidores(IDUsuario) != null)
                {
                    List<Seguidores> seguidores = Seguidores.ListarSeguidores(IDUsuario);
                    ViewBag.Seguidores = seguidores;
                }
                else
                {
                    ViewBag.Erro = "Não foram encontrados seguidores!";
                }

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }
                if (Request.HttpMethod == "POST")
                {
                    Show sh = new Show();

                    sh.Titulo = Request.Form["Titulo"].ToString();
                    sh.Descricao = Request.Form["Descricao"].ToString();
                    sh.Hora = Request.Form["Hora"].ToString();
                    sh.Data = Request.Form["Data"].ToString();
                    //sh.NovoEvento();

                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }


        public ActionResult VerUsuario()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (TempData["Usuario"] != null)
                {
                    Usuario user = new Usuario();
                    user = (Usuario)TempData["Usuario"];

                    ViewBag.Visualizacao = (Usuario)TempData["Usuario"];
                    ViewBag.ArquivosVisualizacao = Models.Arquivos.ListarArquivoUsuario(user.ID,true);
                    ViewBag.InstrumentosVisualizacao = Instrumentos.ListarEspecifico(user.ID, true);

                    //VIEWBAG RELACIONADA AS POSTAGENS DO USUARIO
                    ViewBag.PostagensUsuario = Postagem.ListarPostagem(user.ID);

                }
                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }

            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }
            return View();
        }

        public ActionResult ExcluirInstrumento(int id)
        {
            //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
            Int32 IDUsuario;

            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }
                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                Instrumentos i = new Instrumentos(id, IDUsuario);
                i.Excluir(IDUsuario);
            }
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return RedirectToAction("EditarPerfil", "Menu");
        }

        public ActionResult ExcluirEvento(int id)
        {
            //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
            Int32 IDUsuario;

            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }
                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                Show s = new Show(id);
                s.Excluir();
            }
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return RedirectToAction("Agenda", "Menu");
        }

        public ActionResult EditarPerfil()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
                Int32 IDUsuario;
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                    ViewBag.Instrumentos = Instrumentos.ListarEspecifico(IDUsuario, true);
                    ViewBag.InstrumentosCompleto = Instrumentos.ListarEspecifico(IDUsuario, false);
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                    ViewBag.Instrumentos = Instrumentos.ListarEspecifico(IDUsuario, true);
                    ViewBag.InstrumentosCompleto = Instrumentos.ListarEspecifico(IDUsuario, false);
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                ViewBag.Instruments = Instrumentos.Listar();

                if (Request.HttpMethod == "POST")
                {

                    String NovoNome = Request.Form["NovoNome"];
                    String NovoSobrenome = Request.Form["NovoSobrenome"];
                    String NovoEmail = Request.Form["NovoEmail"];
                    String NovaCidade = Request.Form["NovaCidade"];
                    String NovoEstado = Request.Form["NovoEstado"];
                    String NovoTelefone = Request.Form["NovoCelular"];
                    String NovoTwitch = Request.Form["NovoCanalTwitch"];
                    String NovoYoutube = Request.Form["NovoCanalYoutube"];
                    String NovoFacebook = Request.Form["NovoCanalFacebook"];
                    String NovaBio = Request.Form["NovaBio"];

                    Int32 NovoInstrumento = Convert.ToInt32(Request.Form["NovoInstrumento"]);
                    Int32 NovaMaestria = Convert.ToInt32(Request.Form["NovaMaestria"]);

                    HttpPostedFileBase NovaImagemPerfil = Request.Files["NovaImagemPerfil"];
                    HttpPostedFileBase NovaImagemCapa = Request.Files["NovaImagemCapa"];

                    Usuario EditarUsuario = new Usuario();
                    Instrumentos EditarInstrumento = new Instrumentos();

                    if (Session["Administrador"] != null)
                    {
                        EditarUsuario = (Usuario)Session["Administrador"];
                    }
                    else
                    {
                        EditarUsuario = (Usuario)Session["Usuario"];
                    }

                    //CASO O CAMPO DE NOME SEJA DIFERENTE DE NULO
                    if (NovoNome != "")
                    {
                        EditarUsuario.Nome = NovoNome;
                    }

                    //CASO O CAMPO DE SOBRENOME SEJA DIFERENTE DE NULO
                    if (NovoSobrenome != "")
                    {
                        EditarUsuario.Sobrenome = NovoSobrenome;
                    }

                    //CASO O CAMPO DE EMAIL SEJA DIFERENTE DE NULO
                    if (NovoEmail != "")
                    {
                        EditarUsuario.Email = NovoEmail;
                    }

                    //CASO O CAMPO DE CIDADE SEJA DIFERENTE DE NULO
                    if (NovaCidade != "")
                    {
                        EditarUsuario.Cidade = NovaCidade;
                    }

                    //CASO O CAMPO DE ESTADO SEJA DIFERENTE DE NULO
                    if (NovoEstado != "")
                    {
                        EditarUsuario.Estado = NovoEstado;
                    }

                    //CASO O CAMPO DE BIOGRAFIA SEJA DIFERENTE DE NULO
                    if(NovaBio != "")
                    {
                        EditarUsuario.Biografia = NovaBio;
                    }

                    //CASO O CAMPO DE TELEFONE SEJA DIFERENTE DE NULO
                    if (NovoTelefone != "")
                    {
                        EditarUsuario.Telefone = NovoTelefone;
                    }

                    if(NovoTwitch != "")
                    {
                        EditarUsuario.Twitch = NovoTwitch;
                    }

                    if (NovoYoutube != "")
                    {
                        EditarUsuario.Youtube = NovoYoutube;
                    }

                    if (NovoFacebook != "")
                    {
                        EditarUsuario.Facebook = NovoFacebook;
                    }

                    if (NovoInstrumento != 0 && NovaMaestria != 0)
                    {
                        EditarInstrumento.ID = NovoInstrumento;
                        EditarInstrumento.Maestria = NovaMaestria;
                        EditarInstrumento.Novo(EditarUsuario.ID);
                        
                    }

                    //CASO O CAMPO DE IMAGEM DE PERFIL SEJA DIFERENTE DE NULO
                    if (NovaImagemPerfil.FileName != "")
                    {
                        //PERCORRE OS FILES NO INPUT
                        foreach (string fileName in Request.Files)
                        {
                            //RECOLHE DADOS DO FILE QUE ESTA NO INPUT
                            HttpPostedFileBase postedFile = Request.Files[fileName];
                            int contentLength = postedFile.ContentLength;
                            string contentType = postedFile.ContentType;
                            string nome = postedFile.FileName;
                            int ID = IDUsuario;

                            //CRIA UM OBJETO IMAGEM PARA REDIMENSIONAMENTO
                            Imagem img = new Imagem();

                            //CASO O FILE POSSUA UMA EXTENSAO IGUAL A JPEG OU PNG OU JPG
                            if (contentType.IndexOf("jpeg") > 0 || contentType.IndexOf("png") > 0 || contentType.IndexOf("jpg") > 0)
                            {
                                //FORNECE AS DIMENSOES PARA O REDIMENSIONAMENTO
                                Bitmap arquivoConvertido = img.ResizeImage(postedFile.InputStream, 180, 180);

                                //CRIA O NOME DO ARQUIVO, ESTE QUE TRAS O ID DO USUARIO
                                string nomeArquivoUpload = "imagemPerfil" + ID + ".png";

                                //SALVA O ARQUIVO
                                arquivoConvertido.Save(HttpRuntime.AppDomainAppPath + "\\Imagens\\ImagensUsuario\\" + nomeArquivoUpload);

                                //SETA A IMAGEM DE PERFIL DO USUARIO
                                EditarUsuario.Imagem_Perfil = nomeArquivoUpload;
                            }

                        }
                    }

                    //CASO O CAMPO DE IMAGEM DE CAPA SEJA DIFERENTE DE NULO
                    if (NovaImagemCapa.FileName != "")
                    {
                        //PERCORRE OS FILES NO INPUT
                        foreach (string fileName in Request.Files)
                        {
                            //RECOLHE DADOS DO FILE QUE ESTA NO INPUT
                            HttpPostedFileBase postedFile = Request.Files[fileName];
                            int contentLength = postedFile.ContentLength;
                            string contentType = postedFile.ContentType;
                            string nome = postedFile.FileName;
                            int ID = IDUsuario;

                            //CRIA UM OBJETO IMAGEM PARA REDIMENSIONAMENTO
                            Imagem img = new Imagem();

                            //CASO O FILE POSSUA UMA EXTENSAO IGUAL A JPEG OU PNG OU JPG
                            if (contentType.IndexOf("jpeg") > 0 || contentType.IndexOf("png") > 0 || contentType.IndexOf("jpg") > 0)
                            {
                                //FORNECE AS DIMENSOES PARA O REDIMENSIONAMENTO
                                Bitmap arquivoConvertido = img.ResizeImage(postedFile.InputStream, 1920, 1080);

                                //CRIA O NOME DO ARQUIVO, ESTE QUE TRAS O ID DO USUARIO
                                string nomeArquivoUpload = "imagemCapa" + ID + ".png";

                                //SALVA O ARQUIVO
                                arquivoConvertido.Save(HttpRuntime.AppDomainAppPath + "\\Imagens\\ImagensUsuario\\" + nomeArquivoUpload);

                                //SETA A IMAGEM DE CAPA DO USUARIO
                                EditarUsuario.Imagem_Capa = nomeArquivoUpload;
                            }

                        }
                    }

                    if (EditarUsuario.Alterar())
                    {
                        ViewBag.Mensagem = "Perfil alterado com sucesso!";

                        if (Session["Administrador"] != null)
                        {
                            Session["Administrador"] = EditarUsuario;
                            ViewBag.Usuario = (Usuario)Session["Administrador"];
                        }
                        else {
                            Session["Usuario"] = EditarUsuario;
                            ViewBag.Usuario = (Usuario)Session["Usuario"];
                        }
                        Response.Redirect("~/Menu/Perfil", false);
                    }
                    else
                    {
                        ViewBag.Mensagem = "Houve um erro ao alterar o Perfil. Verifique os dados e tente novamente.";
                    }
                }
                return View();
            }
            Response.Redirect("~/Acesso/Login", false);
            return View();

        }

        public ActionResult EditarBanda()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
                Int32 IDUsuario;
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                    ViewBag.Bandas = Bandas.VisualizarBanda(IDUsuario);
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                    ViewBag.Bandas = Bandas.VisualizarBanda(IDUsuario);
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Request.HttpMethod == "POST")
                {

                    String NovoNome = Request.Form["NovoNome"];
                    String NovaDescricao = Request.Form["NovaDescricao"];
                    HttpPostedFileBase NovaImagemPerfilBanda = Request.Files["NovaImagemPerfilBanda"];
                    HttpPostedFileBase NovaImagemCapaBanda = Request.Files["NovaImagemCapaBanda"];

                    Usuario EditarUsuario = new Usuario();
                    Bandas EditarBandas = new Bandas(ViewBag.Bandas.ID);

                    if (Session["Administrador"] != null)
                    {
                        EditarUsuario = (Usuario)Session["Administrador"];
                    }
                    else
                    {
                        EditarUsuario = (Usuario)Session["Usuario"];
                    }

                    //CASO O CAMPO DE NOME SEJA DIFERENTE DE NULO
                    if (NovoNome != "")
                    {
                        EditarBandas.Nome = NovoNome;
                    }

                    //CASO O CAMPO DE DESCRIÇÃO SEJA DIFERENTE DE NULO
                    if (NovaDescricao != "")
                    {
                        EditarBandas.Descricao = NovaDescricao;
                    }

                    //CASO O CAMPO DE IMAGEM DE PERFIL SEJA DIFERENTE DE NULO
                    if (NovaImagemPerfilBanda.FileName != "")
                    {
                        //PERCORRE OS FILES NO INPUT
                        foreach (string fileName in Request.Files)
                        {
                            //RECOLHE DADOS DO FILE QUE ESTA NO INPUT
                            HttpPostedFileBase postedFile = Request.Files[fileName];
                            int contentLength = postedFile.ContentLength;
                            string contentType = postedFile.ContentType;
                            string nome = postedFile.FileName;
                            
                            //CRIA UM OBJETO IMAGEM PARA REDIMENSIONAMENTO
                            Imagem img = new Imagem();

                            //CASO O FILE POSSUA UMA EXTENSAO IGUAL A JPEG OU PNG OU JPG
                            if (contentType.IndexOf("jpeg") > 0 || contentType.IndexOf("png") > 0 || contentType.IndexOf("jpg") > 0)
                            {
                                //FORNECE AS DIMENSOES PARA O REDIMENSIONAMENTO
                                Bitmap arquivoConvertido = img.ResizeImage(postedFile.InputStream, 180, 180);

                                //CRIA O NOME DO ARQUIVO, ESTE QUE TRAS O ID DO USUARIO
                                string nomeArquivoUpload = "imagemPerfilBanda" + EditarBandas.ID + ".png";

                                //SALVA O ARQUIVO
                                arquivoConvertido.Save(HttpRuntime.AppDomainAppPath + "\\Imagens\\ImagensBandas\\" + nomeArquivoUpload);

                                //SETA A IMAGEM DE PERFIL DO USUARIO
                                EditarBandas.Imagem_Perfil_Banda = nomeArquivoUpload;
                            }

                        }
                    }

                    //CASO O CAMPO DE IMAGEM DE CAPA SEJA DIFERENTE DE NULO
                    if (NovaImagemCapaBanda.FileName != "")
                    {
                        //PERCORRE OS FILES NO INPUT
                        foreach (string fileName in Request.Files)
                        {
                            //RECOLHE DADOS DO FILE QUE ESTA NO INPUT
                            HttpPostedFileBase postedFile = Request.Files[fileName];
                            int contentLength = postedFile.ContentLength;
                            string contentType = postedFile.ContentType;
                            string nome = postedFile.FileName;

                            //CRIA UM OBJETO IMAGEM PARA REDIMENSIONAMENTO
                            Imagem img = new Imagem();

                            //CASO O FILE POSSUA UMA EXTENSAO IGUAL A JPEG OU PNG OU JPG
                            if (contentType.IndexOf("jpeg") > 0 || contentType.IndexOf("png") > 0 || contentType.IndexOf("jpg") > 0)
                            {
                                //FORNECE AS DIMENSOES PARA O REDIMENSIONAMENTO
                                Bitmap arquivoConvertido = img.ResizeImage(postedFile.InputStream, 1920, 1080);

                                //CRIA O NOME DO ARQUIVO, ESTE QUE TRAS O ID DO USUARIO
                                string nomeArquivoUpload = "imagemCapaBanda" + EditarBandas.ID + ".png";

                                //SALVA O ARQUIVO
                                arquivoConvertido.Save(HttpRuntime.AppDomainAppPath + "\\Imagens\\ImagensBandas\\" + nomeArquivoUpload);

                                //SETA A IMAGEM DE CAPA DO USUARIO
                                EditarBandas.Imagem_Capa_Banda = nomeArquivoUpload;
                            }

                        }
                    }

                    EditarBandas.Alterar();
                }
                return View();
            }
            Response.Redirect("~/Acesso/Login", false);
            return View();

        }


        public ActionResult Pesquisar(String busca)
        {
            if (busca == null)
            {
                Response.Redirect("/Menu/Feed");
            }
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                Int32 IDUsuario;
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                //RETORNA OS USUARIOS, CASO HAJA RESULTADO
                if (Usuario.Listar(busca) != null)
                {
                        List<Usuario> Usuarios = Usuario.Listar(busca);

                    for (int i = 0; i <= Usuarios.Count; i++)
                    {
                        //if (Usuarios[i].Modo == true)
                        //{
                        //    Usuarios.Remove(Usuarios[i]);
                        //}
                    }

                    ViewBag.Usuarios = Usuarios;

                }

                if (Bandas.ListarBandas(busca) != null)
                {
                    List<Bandas> Bands = Bandas.ListarBandas(busca);
                    ViewBag.Bandas = Bands;
                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }

        public ActionResult MostrarUsuario(int id)
        {
            Usuario usuarios = new Usuario(id);
            if (usuarios != null)
            {
                TempData["Usuario"] = usuarios;

                return RedirectToAction("VerUsuario", "Menu");
            }
            else
            {
                return RedirectToAction("Pesquisar", "Menu");
            }
        }
       
        public ActionResult Livestream()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }

        public ActionResult VerSeguidores()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }
                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Seguidores.ListarSeguidores(IDUsuario) != null)
                {
                    List<Seguidores> seguidores = Seguidores.ListarSeguidores(IDUsuario);
                    ViewBag.Seguidores = seguidores;
                }
                else
                {
                    ViewBag.Erro = "Não foram encontrados seguidores!";
                }

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }

        public ActionResult VerSeguidos()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Seguidores.ListarSeguidos(IDUsuario) != null)
                {
                    List<Seguidores> seguidos = Seguidores.ListarSeguidos(IDUsuario);
                    ViewBag.Seguidos = seguidos;
                }
                else
                {
                    ViewBag.Erro = "Não foram encontrados seguidores!";
                }

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }

        public ActionResult VerBandas()
        {

            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Bandas.MostrarBanda(IDUsuario) != null)
                {
                    //CRIA UMA BANDA CASO O USUARIO FAÇA PARTE DE UMA
                    Bandas banda = new Bandas();
                    banda = Bandas.MostrarBanda(IDUsuario);

                    //VERIFICA SE A BANDA POSSUI INTEGRANTES
                    if (Integrantes.ListarIntegrantes(banda.ID) != null)
                    {
                        //CASO O USUARIO POSSUA UMA BANDA, SERA FEITA UMA BUSCA DOS INTEGRATENS DA MESMA
                        List<Integrantes> integrantes = Integrantes.ListarIntegrantes(banda.ID);
                        ViewBag.Integrantes = integrantes;
                    }
                }
                else
                {
                    ViewBag.Erro = "Não foram encontrados os INTEGRANTES!";
                }

                // VERIFICA SE O USUARIO SEGUE ALGUEM PARA ADICIONAR INTEGRANTE
                if (Seguidores.ListarSeguidos(IDUsuario) != null)
                {
                    List<Seguidores> seguidos = Seguidores.ListarSeguidos(IDUsuario);
                    ViewBag.Seguidos = seguidos;
                }
                else
                {
                    ViewBag.Erro = "Não foram encontrados seguidores!";
                }

                Bandas bandas = new Bandas();
                bandas = Bandas.MostrarBanda(IDUsuario);


                // VERIFICA SE EXISTE ALGUMA BANDA
                if (Bandas.MostrarBanda(IDUsuario) != null)
                {
                   
                    // VERIFICAÇÃO
                    if (Bandas.ListarBanda(bandas.ID) != null)
                    {
                        Bandas banda = new Bandas();
                        banda = Bandas.ListarBanda(bandas.ID);
                        ViewBag.Bandas = banda;
                    }
                    else
                    {
                        ViewBag.Erro = "Não foram encontrados as bandas!";
                    }
                }
                
                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return View();
        }

        public ActionResult AdicionarIntegrante(int ID)
        {
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Bandas.MostrarBanda(IDUsuario) != null)
                {
                    Bandas bandas = new Bandas();
                    bandas = Bandas.MostrarBanda(IDUsuario);
                    Usuario novoIntegrante = new Usuario(ID);
                    bandas.AdicionarIntegrante(novoIntegrante.ID);
                }

            }
            return RedirectToAction("VerBandas","Menu");
        }

        public ActionResult ExcluirIntegrante(int ID)
        {
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                //CRIA VARIAVEL GLOBAL DO ID DO USUARIO
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                if (Bandas.MostrarBanda(IDUsuario) != null)
                {
                    Bandas bandas = new Bandas();
                    bandas = Bandas.MostrarBanda(IDUsuario);
                    bandas.RemoverIntegrante(ID);
                }

            }
            return RedirectToAction("VerBandas", "Menu");
        }

        public ActionResult Unfollow(Int32 ID)
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                }

                Seguidores.Unfollow(ID);
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return RedirectToAction("VerSeguidos", "Menu");
        }

        public ActionResult Follow(Int32 ID)
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                Int32 IDUsuario;

                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                Seguidores.Follow(IDUsuario, ID);
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return RedirectToAction("VerSeguidos", "Menu");
        }

        public ActionResult Email()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                }

                if (Request.HttpMethod == "POST")
                {

                    /**Parte de enviar o email**/
                    MailMessage msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient("smtp.office365.com");
                    msg.From = new MailAddress("contato12345123451@outlook.com");
                    msg.To.Add(Request.Form["email"].ToString());
                    msg.Subject = "Recuperação de Senha";
                    msg.Body = "sua nova senha é: ";

                    /**contato616516@outlook.com**/
                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("contato12345123451@outlook.com", "senai1234");
                    smtp.EnableSsl = true;
                }

                //METODO PARA BUSCA DE USUARIOS, MUSICAS, BANDAS
                if (Request.HttpMethod == "POST")
                {
                    String busca = Request.Form["busca"].ToString();
                    Response.Redirect("/Menu/Pesquisar/" + busca);
                }

            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }
            return View();
        }
        public ActionResult Arquivo()
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                Int32 IDUsuario;
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                    IDUsuario = User.ID;
                }

                //VIEWBAG RELACIONADAS AO RANKING DE USUARIOS
                ViewBag.Ranking = Usuario.Ranquear(true);

                //VIEWBAGS RELACIONADAS AOS ADMINISTRADORES DO SITE
                ViewBag.QntADM = Usuario.ContarADM();
                ViewBag.Administradores = Usuario.ListarADM();

                if (Request.HttpMethod == "POST")
                {
                    foreach (string fileName in Request.Files)
                    {
                        HttpPostedFileBase postedFile = Request.Files[fileName];
                        int contentLength = postedFile.ContentLength;
                        string contentType = postedFile.ContentType;

                        String nome = postedFile.FileName;
                        String nomeArquivo = null;

                        Arquivos arquivo = new Arquivos();

                        for(int i=0; i <= nome.Length; i++)
                        {
                            if(i == nome.IndexOf('.'))
                            {
                                //REMOVE OS CARACTERES APOS O PONTO(EXTENSAO DO ARQUIVO)
                                DateTime data = DateTime.Now;

                                nomeArquivo = data.Hour + data.Minute + data.Second + data.Millisecond + "_" + nome.Substring(0, nome.IndexOf('.'))+ ".mp3";
                            }
                        }

                        if (contentType.IndexOf("mp3") > 0 || contentType.IndexOf("audio") > 0 || contentType.IndexOf("mpeg") > 0 || contentType.IndexOf("wav") > 0) 
                        { 
                            postedFile.SaveAs(HttpRuntime.AppDomainAppPath + "//Arquivos//" + nomeArquivo);
                            arquivo.NovoArquivo(contentType, nomeArquivo, IDUsuario);
                        }
                        // else
                        //  postedFile.SaveAs(@"C:\Users\16128604\Source\Repos\lpw-2017-3infb-g4\Katiau\WebSite\images\" + Request.Form["Desc"] + ".txt");

                    }


                }

            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }
            return View();
        }

        public ActionResult Avaliar(Int32 ID)
        {
            //VERIFICA SE EXISTE ALGUM DADO NA SESSÃO USUARIO
            if (Session["Usuario"] != null || Session["Administrador"] != null)
            {
                if (Session["Administrador"] != null)
                {
                    //CRIA SESSÃO DO Administrador
                    ViewBag.Logado = Session["Administrador"];
                    Usuario User = (Usuario)Session["Administrador"];
                    ViewBag.User = User;
                }

                else
                {
                    //CRIA SESSÃO DO USUARIO
                    ViewBag.Logado = Session["Usuario"];
                    Usuario User = (Usuario)Session["Usuario"];
                    ViewBag.User = User;
                }

                Usuario.Avaliar(ID);
            }
            //CASO SESSAO SEJA NULA -> REDIRECIONAMENTO PARA PAGINA LOGIN
            else
            {
                Response.Redirect("/Acesso/Login");
            }

            return RedirectToAction("VerUsuario", "Menu");
        }

    }
}
