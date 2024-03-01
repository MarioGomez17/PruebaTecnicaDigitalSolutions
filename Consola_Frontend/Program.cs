using API_Backend.Modelos;
using Newtonsoft.Json;

while (true)
{
    string ComandoIngresado = Console.ReadLine();
    Comando Comando = new();
    Comando.CrearComando(ComandoIngresado);
    CarcarComando(Comando);
};

static void CarcarComando(Comando Comando)
{
    switch (Comando.PrimeraParte.ToLower())
    {
        case "post":
            MetodoPOST(Comando);
            break;
        case "follow":
            MetodoFOLLOW(Comando);
            break;
        case "dashboard":
            MetodoDASHBOARD(Comando);
            break;
        case "load":
            string Ruta = Comando.SegundaParte.Substring(1, Comando.SegundaParte.Length - 2);
            if (File.Exists(Ruta))
            {
                using StreamReader sr = new StreamReader(Ruta);
                string Linea;
                while ((Linea = sr.ReadLine()) != null)
                {
                    Comando ComandoArchivo = new();
                    ComandoArchivo.CrearComando(Linea);
                    CarcarComando(ComandoArchivo);
                }
            }
            else
            {
                Console.WriteLine("EL ARCHIVO NO EXISTE EN LA RUTA ESPECIFICADA");
            }
            break;
        default:
            Console.WriteLine("COMANDO NO VÁLIDO");
            break;
    }
}
static async void MetodoPOST(Comando Comando)
{
    if (Comando.SegundaParte[0] == '#')
    {
        string NombreCrearUsuario = Comando.TerceraParte;
        string URL_Usuario = "https://localhost:7156/Usuario/CrearUsuario?Nombre=" + NombreCrearUsuario;
        using (HttpClient client = new())
        {
            try
            {
                HttpResponseMessage response = await client.PostAsync(URL_Usuario, null);
                if (response.IsSuccessStatusCode)
                {
                    string RespuestaUsuario = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(RespuestaUsuario);
                }
            }
            catch { }
        }

    }
    else if (Comando.SegundaParte[0] == '@')
    {
        string NombreHacerPublicacion = Comando.SegundaParte[1..];
        string Mensaje = Comando.TerceraParte;
        string URL_Publicacion = "https://localhost:7156/Publicacion/HacerPublicacion?Nombre=" + NombreHacerPublicacion + "&Mensaje=" + Mensaje;
        using (HttpClient client = new())
        {
            try
            {
                HttpResponseMessage response = await client.PostAsync(URL_Publicacion, null);
                if (response.IsSuccessStatusCode)
                {
                    string RespuestaPublicacion = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(RespuestaPublicacion);
                }
            }
            catch { }
        }
    }
}

static async void MetodoFOLLOW(Comando Comando)
{
    string Seguidor = Comando.SegundaParte[1..];
    string Seguido = Comando.TerceraParte[1..];
    string URL_Seguir = "https://localhost:7156/Usuario/SeguirUsuario?NombreSeguidor=" + Seguidor + "&NombreSeguido=" + Seguido;
    using (HttpClient client = new())
    {
        try
        {
            HttpResponseMessage response = await client.PostAsync(URL_Seguir, null);
            if (response.IsSuccessStatusCode)
            {
                string RespuestaSeguir = await response.Content.ReadAsStringAsync();
                Console.WriteLine(RespuestaSeguir);
            }
        }
        catch { }
    }
}

static async void MetodoDASHBOARD(Comando Comando)
{
    String Dashboard = "";
    string NombreDasboard = Comando.SegundaParte[1..];
    string URL_TraerSeguidos = "https://localhost:7156/Usuario/TraerSeguidos?Nombre=" + NombreDasboard;
    using (HttpClient client = new())
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(URL_TraerSeguidos);
            if (response.IsSuccessStatusCode)
            {
                string JsonSeguidos = await response.Content.ReadAsStringAsync();
                dynamic DataSeguidos = JsonConvert.DeserializeObject(JsonSeguidos);
                foreach (int Dato in DataSeguidos)
                {
                    string URL_Dashboard = "https://localhost:7156/Publicacion/TraerPublicaciones?IdUsuario=" + Dato;
                    using (HttpClient clientDashboard = new())
                    {
                        try
                        {
                            HttpResponseMessage responseDashboard = await client.GetAsync(URL_Dashboard);
                            if (response.IsSuccessStatusCode)
                            {
                                string RespuestaDashboard = await responseDashboard.Content.ReadAsStringAsync();
                                Dashboard += RespuestaDashboard;
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        catch { }
    }
    Console.WriteLine(Dashboard);
}