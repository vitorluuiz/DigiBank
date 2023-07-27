using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace digibank_back.Utils
{
    public class Upload
    {
        public static string RetornarExtensao(string nomeDoArquivo)
        {
            string[] dados = nomeDoArquivo.Split(".");
            return dados[dados.Length - 1];
        }
        public static void RemoverArquivo(string nomeDoArquivo)
        {
            var pasta = Path.Combine("StaticFiles", "Images");
            var caminho = Path.Combine(Directory.GetCurrentDirectory(), pasta);
            var caminhoCompleto = Path.Combine(caminho, nomeDoArquivo);

            File.Delete(caminhoCompleto);
        }

        public static bool ValidarExtensao(string[] extensoes, string nomeDoArquivo)
        {
            string[] dados = nomeDoArquivo.Split(".");
            string extensao = dados[dados.Length - 1];

            foreach (var item in extensoes)
            {
                if (extensao == item)
                {
                    return true;
                }
            }
            return false;
        }

        public static string UploadFiles(List<IFormFile> arquivos, string[] extensoesPermitidas, byte idProduto)
        {
            try
            {
                var pasta = Path.Combine("StaticFiles", "Images");
                var caminho = Path.Combine(Directory.GetCurrentDirectory(), pasta);
                if (arquivos.Count > 0)
                {
                    List<string> caminhos = new List<string>();
                    int countErrors = 0;

                    foreach (IFormFile arquivo in arquivos)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');

                        if (ValidarExtensao(extensoesPermitidas, fileName))
                        {
                            var extensao = RetornarExtensao(fileName);
                            var novoNome = $"{Guid.NewGuid()}.{extensao}";
                            var caminhoCompleto = Path.Combine(caminho, novoNome);

                            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                            {
                                arquivo.CopyTo(stream);
                            }
                            caminhos.Add(novoNome.ToString());
                        }
                        else
                        {
                            countErrors++;
                        }
                    }
                    ImgsProdutoRepository imgRepository = new ImgsProdutoRepository();
                    imgRepository.CadastrarCaminhos(idProduto, caminhos);
                    return countErrors > 0 ? $"{countErrors} erros encontrados ao cadastrar imagens, verifique se seu arquivo possuí uma extensão permitida" : "Nenhum erro encontrado";
                }
                return "Sem arquivos";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string UploadFile(IFormFile arquivo, string[] extensoesPermitidas)
        {
            try
            {
                var pasta = Path.Combine("StaticFiles", "Images");
                var caminho = Path.Combine(Directory.GetCurrentDirectory(), pasta);

                if (arquivo != null)
                {
                    if (arquivo.Length > 0)
                    {
                        var fileName = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');

                        if (ValidarExtensao(extensoesPermitidas, fileName))
                        {
                            var extensao = RetornarExtensao(fileName);
                            var novoNome = $"{Guid.NewGuid()}.{extensao}";
                            var caminhoCompleto = Path.Combine(caminho, novoNome);

                            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                            {
                                arquivo.CopyTo(stream);
                            }

                            return novoNome;
                        }

                        return "Extensão não permitida";
                    }
                }
                return "Sem arquivo";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
