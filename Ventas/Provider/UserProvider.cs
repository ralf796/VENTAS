using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace Ventas.Provider
{
    public class UserProvider
    {
        public SesionUsuario Usuario { get; } = new SesionUsuario();

        public UserProvider(ClaimsPrincipal user)
        {
            try
            {


                #region CodigoEmpresa
                var lstEmpresa = user.Claims.Where(c => c.Type == ClaimTypes.Locality)
                                  .Select(c => c.Value);
                if (lstEmpresa.Count() < 1)
                {
                    Usuario.Cod_emp = "ND";
                }
                else
                {
                    var codempresa = lstEmpresa.First();
                    Usuario.Cod_emp = (codempresa == null ? "" : codempresa.ToString());
                }
                #endregion

                #region CodigoPais
                var lstPais = user.Claims.Where(c => c.Type == ClaimTypes.Country)
                                  .Select(c => c.Value);
                if (lstPais.Count() < 1)
                {
                    Usuario.Cod_Pais = "ND";
                }
                else
                {
                    var codpais = lstPais.First();
                    Usuario.Cod_Pais = (codpais == null ? "" : codpais.ToString());
                }
                #endregion

                #region Rol
                var lstRol = user.Claims.Where(c => c.Type == ClaimTypes.Role)
                                  .Select(c => c.Value);
                if (lstRol.Count() < 1)
                {
                    Usuario.Rol_nombre = "ND";
                }
                else
                {
                    var rol = lstRol.First();
                    Usuario.Rol_nombre = (rol == null ? "" : rol.ToString());
                }
                #endregion

                #region Nombrecompleto
                var lstNombreCompleto = user.Claims.Where(c => c.Type == ClaimTypes.GivenName)
                                  .Select(c => c.Value);
                if (lstNombreCompleto.Count() < 1)
                {
                    Usuario.Usuario_nombre_completo = "ND";
                }
                else
                {
                    var usuarionombrecompleto = lstNombreCompleto.First();
                    Usuario.Usuario_nombre_completo = (usuarionombrecompleto == null ? "" : usuarionombrecompleto.ToString());
                }
                #endregion

                #region UsuarioNombre
                var lstUsuarioNombre = user.Claims.Where(c => c.Type == ClaimTypes.Name)
                                  .Select(c => c.Value);
                if (lstUsuarioNombre.Count() < 1)
                {
                    Usuario.Usuario_nombre = "ND";
                }
                else
                {
                    var usuarionombre = lstUsuarioNombre.First();
                    Usuario.Usuario_nombre = (usuarionombre == null ? "" : usuarionombre.ToString());
                }
                #endregion

                #region Token
                var lsttoken = user.Claims.Where(c => c.Type == ClaimTypes.Hash)
                   .Select(c => c.Value);
                if (lsttoken.Count() < 1)
                {
                    Usuario.Token = "ND";
                }
                else
                {
                    var token = lsttoken.First();
                    Usuario.Token = (token == null ? "" : token.ToString());
                }

                #endregion

                #region IP
                var lstip = user.Claims.Where(c => c.Type == ClaimTypes.System)
                   .Select(c => c.Value);
                if (lstip.Count() < 1)
                {
                    Usuario.Ip = "ND";
                }
                else
                {
                    var ip = lstip.First();
                    Usuario.Ip = (ip == null ? "" : ip.ToString());
                }
                #endregion

                #region Pais
                var lstpais = user.Claims.Where(c => c.Type == ClaimTypes.GroupSid)
                   .Select(c => c.Value);
                if (lstpais.Count() < 1)
                {
                    Usuario.Pais = "ND";
                }
                else
                {
                    var pais = lstpais.First();
                    Usuario.Pais = (pais == null ? "" : pais.ToString());
                }
                #endregion

                #region CodDepartamento
                var lstcoddepartamento = user.Claims.Where(c => c.Type == ClaimTypes.Actor)
                                  .Select(c => c.Value);
                if (lstcoddepartamento.Count() < 1)
                {
                    Usuario.CodDepartamento = "ND";
                }
                else
                {
                    var coddepartamento = lstcoddepartamento.First();
                    Usuario.CodDepartamento = (coddepartamento == null ? "" : coddepartamento.ToString());
                }
                #endregion

                #region TokenApp
                var ltokapp = user.Claims.Where(c => c.Type == ClaimTypes.Actor)
                                  .Select(c => c.Value);
                if (ltokapp.Count() < 1)
                {
                    Usuario.TokenApp = "";
                }
                else
                {
                    var tokapp = ltokapp.First();
                    Usuario.TokenApp = (tokapp == null ? "" : tokapp.ToString());
                }
                #endregion

                #region TipoLogin
                var ltipolog = user.Claims.Where(c => c.Type == ClaimTypes.Gender)
                                  .Select(c => c.Value);
                if (ltipolog.Count() < 1)
                {
                    Usuario.TipoLogin = "";
                }
                else
                {
                    var tipolog = ltipolog.First();
                    Usuario.TipoLogin = (tipolog == null ? "" : tipolog.ToString());
                }
                #endregion

                #region Cod_trib
                var lcodtrib = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                                  .Select(c => c.Value);
                if (lcodtrib.Count() < 1)
                {
                    Usuario.Cod_trib = "";
                }
                else
                {
                    var codtrib = lcodtrib.First();
                    Usuario.Cod_trib = (codtrib == null ? "" : codtrib.ToString());
                }
                #endregion

            }
            catch (Exception ex)
            {

            }
        }
    }
}