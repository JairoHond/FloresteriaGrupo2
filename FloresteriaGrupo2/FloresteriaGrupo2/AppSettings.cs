using AppNotas.Modelo;
using System;
using System.Collections.Generic;
using System.Text;

namespace FloresteriaGrupo2
{
    public class AppSettings
    {
        public static string ApiFirebase = "https://appfloristeria-default-rtdb.firebaseio.com/";
        public static string KeyAplication = "AIzaSyC-O1SLFIfJsBOUi6kAGnXsX8FGhpnWDMA";


        public static ResponseAuthentication oAuthentication { get; set; }
        private static string ApiUrlGoogleApis = "https://identitytoolkit.googleapis.com/v1/";

        public static string ApiAuthentication(string tipo)
        {
            if (tipo == "LOGIN")
                return ApiUrlGoogleApis + "accounts:signInWithPassword?key=" + KeyAplication;
            else if (tipo == "SIGNIN")
                return ApiUrlGoogleApis + "accounts:signUp?key=" + KeyAplication;
            else
                return "";
        }

    }
}
