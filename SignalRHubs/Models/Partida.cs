namespace SignalRHubs.Models
{
    public class Partida
    {
        public string NombrePartida { get; set; } = null!;
        public string NombreUsuario1 { get; set; } = null!;
        public string NombreUsuario2 { get; set; } = null!;
        public string ConnectionId1 { get; set; } = null!;
        public string ConnectionId2 { get; set; } = null!;
        public char Turno { get; set; }
    }
}