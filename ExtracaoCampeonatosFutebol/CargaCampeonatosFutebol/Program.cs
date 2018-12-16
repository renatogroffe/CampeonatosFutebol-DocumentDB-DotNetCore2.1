using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CargaCampeonatosFutebol
{
    class Program
    {
        private static SeleniumConfigurations _seleniumConfigurations;
        private static DocumentDBConfigurations _documentDBConfigurations;

        static void Main(string[] args)
        {
            Console.WriteLine(
                "*** Extração de Dados da Web com " +
                ".NET Core 2.1, Selenium WebDriver e DocumentDB ***");
            Console.WriteLine("Carregando configurações...");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json");
            var configuration = builder.Build();

            _seleniumConfigurations = new SeleniumConfigurations();
            new ConfigureFromConfigurationOptions<SeleniumConfigurations>(
                configuration.GetSection("SeleniumConfigurations"))
                    .Configure(_seleniumConfigurations);

            _documentDBConfigurations = new DocumentDBConfigurations();
            new ConfigureFromConfigurationOptions<DocumentDBConfigurations>(
                configuration.GetSection("DocumentDBConfigurations"))
                    .Configure(_documentDBConfigurations);

            ExtrairDadosCampeonato("Brasileirão", "Brasil",
                _seleniumConfigurations.UrlPaginaClassificacaoBrasileirao);
            ExtrairDadosCampeonato("La Liga", "Espanha",
                _seleniumConfigurations.UrlPaginaClassificacaoLaLiga);
            ExtrairDadosCampeonato("Premier League", "Inglaterra",
                _seleniumConfigurations.UrlPaginaClassificacaoPremierLeague);

            Console.WriteLine(
                Environment.NewLine +
                "Carga de dados concluída com sucesso!");

            Console.ReadKey();
        }

        private static void ExtrairDadosCampeonato(
            string nomeCampeonato, string pais, string urlClassificacao)
        {
            Console.Write(Environment.NewLine);
            Console.WriteLine(
                "Carregando driver do Selenium para Chrome em modo headless...");
            var paginaClassificacao = new PaginaClassificacao(
                _seleniumConfigurations,
                nomeCampeonato, pais, urlClassificacao);

            Console.WriteLine(
                $"Carregando página com classificações - {nomeCampeonato}...");
            paginaClassificacao.CarregarPagina();

            Console.WriteLine(
                "Extraindo dados...");
            var classificacao = paginaClassificacao.ObterClassificacao();
            paginaClassificacao.Fechar();

            Console.WriteLine("Gravando dados extraídos...");
            new ClassificacaoRepository(_documentDBConfigurations)
                .Incluir(classificacao);
        }
    }
}