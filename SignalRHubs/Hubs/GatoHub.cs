using Microsoft.AspNetCore.SignalR;
using SignalRHubs.Models;

namespace SignalRHubs.Hubs
{
    public class GatoHub : Hub
    {
        public static Dictionary<string, string> usuarios = new();
        public static Dictionary<string, Partida> partidas = new();

        public async Task IniciarSesion(string nombreUsuario)
        {
            //Verifica si el nombre de usuario está en uso
            if (usuarios.Keys.Any(x => x.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase)))
            {
                //Enviar mensaje de error
                await Clients.Caller.SendAsync("ReceiveMessage", "Error", "El nombre del usuario ya está en uso.");
            }
            else
            {
                usuarios[nombreUsuario] = Context.ConnectionId;
                await Clients.Caller.SendAsync("ReceiveMessage","Ok","Sesión iniciada.");
            }

        }

        public static Queue<string> colaUsuarios = new();

        public static int NumPartida = 0;

        //public Dic

        public async Task BuscarPartida(string nombreUsuario)
        {
            if (colaUsuarios.Count == 0)
            {
                colaUsuarios.Enqueue(nombreUsuario);
            }
            else
            {
                var contrincante = colaUsuarios.Dequeue();
                string partida = $"Partida: {NumPartida}";
                await Groups.AddToGroupAsync(Context.ConnectionId, partida);
                await Groups.AddToGroupAsync(usuarios[contrincante], partida);
                NumPartida++;
                await Clients.Groups(partida).SendAsync("GameStart", partida);
                await Clients.Users(Context.ConnectionId).SendAsync("Play");
                var datosPartida = new Partida()
                {
                    NombrePartida = partida,
                    NombreUsuario1 = nombreUsuario,
                    ConnectionId1 = Context.ConnectionId,
                    NombreUsuario2 = contrincante,
                    ConnectionId2 = usuarios[contrincante],
                    Turno = 'X'
                };
                partidas[partida] = datosPartida;
            }

        }

        public async Task Jugar(string partida, string nombreUsuario)
        {
            var datosPartida = partidas[partida];
            if (GanaXO(datosPartida.Turno,tablero))
            {
                //Notificar que ganó
            }
            else
            {
                datosPartida.Turno = (datosPartida.Turno == 'X') ? 'O' : 'X';
            }
        }

        int[,] lineas = new int[,]
        {

        };
    }
}
