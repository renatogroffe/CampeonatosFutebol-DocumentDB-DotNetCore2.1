using System;
using System.Collections.Generic;

namespace CargaCampeonatosFutebol
{
    public class Classificacao
    {
        public string id { get; set; }
        public string NomeCampeonato { get; set; }
        public string Temporada { get; set; }
        public string Esporte { get; set; }
        public string Pais { get; set; }
        public DateTime DataExtracao { get; set; }
        public List<Equipe> Equipes { get; set; } = new List<Equipe>();
    }

    public class Equipe
    {
        public int Posicao { get; set; }
        public string Nome { get; set; }
        public int Jogos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolsPro { get; set; }
        public int GolsContra { get; set; }
        public int SaldoGols { get; set; }
        public int Pontos { get; set; }
    }
}
