using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DipCmiGT.LogicaComun.Util
{
    public static class ValidaciónNIT
    {
        public static bool ValidarNit(string nit)
        {
            try
            {
                //NIT sin guiones 


                //string nit1 = nit.Substring(nit.Length - 1, 1);
                string nit1 = Left(nit, nit.Length - 1);
                string nit2 = Right(nit,1);
                nit = nit1 + '-' + nit2;
                //FIN NIT sin guiones

                Int32 posicion = nit.IndexOf("-");
                string correlativo = nit.Substring(0, posicion);
                string digitoVerificador = nit.Substring(posicion + 1, nit.Length - posicion - 1);
                Int32 factor = correlativo.Length + 1;
                Int32 valor = 0;
                float residuo = 0;
                float resultado = 0;

                for (Int32 i = 0; i < posicion; i++)
                {
                    valor += Convert.ToInt32(correlativo[i].ToString()) * factor;
                    factor -= 1;
                }

                residuo = valor % 11;
                resultado = 11 - residuo;
                //
                resultado = resultado % 11;
                if (resultado == 10)
                {
                    if (digitoVerificador.ToUpper() == "K")
                        return true;
                }
                else if (digitoVerificador == resultado.ToString())
                    return true;

                return false;
            }
            catch (ExcepcionesDIPCMI ex)
            {
                return false;
            }
        }


        public static string Left(string param, int length)
        {

            string result = param.Substring(0, length);
            return result;
        }

        public static string Right(string param, int length)
        {

            int value = param.Length - length;
            string result = param.Substring(value, length);
            return result;
        }
    }
}
