namespace API_Backend.Modelos
{
    public class Comando
    {

        public string PrimeraParte { get; set; }

        public string SegundaParte { get; set; }

        public string TerceraParte { get; set; }

        public void CrearComando(String ComandoIngresado)
        {
            if (ComandoIngresado.Split().Length > 2)
            {
                string[] PartesComando = ComandoIngresado.Split(' ', 3);
                this.PrimeraParte = PartesComando[0];
                this.SegundaParte = PartesComando[1];
                this.TerceraParte = PartesComando[2];
            }
            else
            {
                string[] PartesComando = ComandoIngresado.Split();
                this.PrimeraParte = PartesComando[0];
                this.SegundaParte = PartesComando[1];
            }
        }

    }
}
