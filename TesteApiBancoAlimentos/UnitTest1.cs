using api_web_service_bma.Controllers;
using api_web_service_bma.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using NUnit.Framework.Interfaces;
using System;

namespace TesteApiBancoAlimentos
{
    [TestFixture]
    public class BeneficiarioTeste
    {
        private AppDbContext _dbContext;
        private BeneficiarioController _controller;

        [SetUp]
        public void SetUp()
        {
            //Cria banco em memória para o teste
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase").Options;

            //Cria uma instância do banco e do controlador para serem testatdos
            _dbContext = new AppDbContext(options);
            _controller = new BeneficiarioController(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            //Serve para limpar o banco criado em memória após execução
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetById_BeneficiarioEsperado()
        {//Declara um método assíncrono com retorno Task

            //Arrange
            //Define os dados do teste
            var id = 15;
            string actualNome = "Pietra Ester Ferreira";
            string actualCpf = "32989238697";

            var beneficiarioEsperado = new Beneficiario {
                Id = id,
                Nome = "Pietra Ester Ferreira",
                Cpf = "32989238697",
                DataNascimento = new DateTime(1977, 01, 15),
                Email = "",
                Telefone = "33997690408",
                Cep = "39880972",
                Logradouro = "Rua Carneirinho Antonio Soares 157",
                Numero = "838",
                Complemento = "",
                Bairro = "Água Quente",
                Cidade = "Águas Formosas",
                Uf = "MG",
                situacao = api_web_service_bma.Enum.SituacaoEnum.Ativo,
                tipoCesta = api_web_service_bma.Enum.TipoCestaEnum.BASICA
            };
            //Adiciona o objeto ao conteto do banco e salva as alterações
            _dbContext.Beneficiarios.Add(beneficiarioEsperado);
            _dbContext.SaveChanges();


            //Act
            //Testa o método GetById
            var result = await _controller.GetById(id);


            //Assert
            //Verifica se o resultado da ação é um objeto
            //quando o resultado da ação é um HTTP '200 ok'
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var actualModel = okResult.Value as Beneficiario;
            

            Assert.IsNotNull(actualModel);
            Assert.That(actualModel.Id, Is.EqualTo(beneficiarioEsperado.Id));
            Assert.That(actualNome, Is.EqualTo(beneficiarioEsperado.Nome));
            Assert.That(actualCpf, Is.EqualTo(beneficiarioEsperado.Cpf));

        }

        [Test]
        public async Task GetAll()
        {
            //Arrange
            //Define os dados do teste
            var listaBeneficiario = new List<Beneficiario>
            {
               new Beneficiario {
                Id = 15,
                Nome = "Pietra Ester Ferreira",
                Cpf = "32989238697",
                DataNascimento = new DateTime(1977, 01, 15),
                Email = "",
                Telefone = "33997690408",
                Cep = "39880972",
                Logradouro = "Rua Carneirinho Antonio Soares 157",
                Numero = "838",
                Complemento = "",
                Bairro = "Água Quente",
                Cidade = "Águas Formosas",
                Uf = "MG",
                situacao = api_web_service_bma.Enum.SituacaoEnum.Ativo,
                tipoCesta = api_web_service_bma.Enum.TipoCestaEnum.BASICA
               },
                new Beneficiario {
                Id = 16,
                Nome = "Camila Gabriela Luciana Baptista",
                Cpf = "39566573650",
                DataNascimento = new DateTime(1996, 04, 03),
                Email = "camila_gabriela_baptista@br.gestant.com",
                Telefone = "31993792790",
                Cep = "35410970",
                Logradouro = "Praça Ramos 35-A",
                Numero = "475",
                Complemento = "",
                Bairro = "Centro",
                Cidade = "Cachoeira do Campo",
                Uf = "MG",
                situacao = api_web_service_bma.Enum.SituacaoEnum.Ativo,
                tipoCesta = api_web_service_bma.Enum.TipoCestaEnum.VERDE
               },             
            };

            _dbContext.Beneficiarios.AddRange(listaBeneficiario);
            _dbContext.SaveChanges();

            //Act
            var result =await _controller.GetAll();

            //Assert
            Assert.IsAssignableFrom<OkObjectResult>(result);
            var benefResult = ((OkObjectResult)result).Value as List<Beneficiario>;

            Assert.That(benefResult, Is.EqualTo(listaBeneficiario));

        }

        [Test]
        public async Task Create()
        {
            //Arrange
            //Define os dados do teste
            var beneficiario = new Beneficiario
            {
                Nome = "Pietra Ester Ferreira",
                Cpf = "32989238697",
                DataNascimento = new DateTime(1977, 01, 15),
                Email = "",
                Telefone = "33997690408",
                Cep = "39880972",
                Logradouro = "Rua Carneirinho Antonio Soares 157",
                Numero = "838",
                Complemento = "",
                Bairro = "Água Quente",
                Cidade = "Águas Formosas",
                Uf = "MG",
                situacao = api_web_service_bma.Enum.SituacaoEnum.Ativo,
                tipoCesta = api_web_service_bma.Enum.TipoCestaEnum.BASICA
            };
                        
            //Act
            //Testa o método Create
            var result = await _controller.Create(beneficiario);


            //Assert
            //verifica se o resultado retornado pelo método é uma instância da classe CreatedAtActionResult 
            Assert.IsInstanceOf<CreatedAtActionResult>(result); 
            
            //converte o resultado para que possamos acessar suas propriedades
            var okResult = result as CreatedAtActionResult;
            
            //compara o objeto criado com o valor da propriedade Value do resultado retornado
            Assert.That(okResult.Value, Is.EqualTo(beneficiario));
           
            //Verifica se o objeto está presente no banco de dados
            var savedObj = await _dbContext.Beneficiarios.FirstOrDefaultAsync(o => o.Id == beneficiario.Id);
            Assert.That(savedObj.Nome, Is.EqualTo(beneficiario.Nome));

        }

        [Test]
        public async Task Update()
        {
            //Arrange
            //Define os dados do teste
            int id = 10;

            var beneficiario = new Beneficiario
            {
                Id = id, 
                Nome = "Pietra Ester Ferreira",
                Cpf = "32989238697",
                DataNascimento = new DateTime(1977, 01, 15),
                Email = "",
                Telefone = "33997690408",
                Cep = "39880972",
                Logradouro = "Rua Carneirinho Antonio Soares 157",
                Numero = "838",
                Complemento = "",
                Bairro = "Água Quente",
                Cidade = "Águas Formosas",
                Uf = "MG",
                situacao = api_web_service_bma.Enum.SituacaoEnum.Ativo,
                tipoCesta = api_web_service_bma.Enum.TipoCestaEnum.BASICA
            };

            await _dbContext.Beneficiarios.AddAsync(beneficiario);
            await _dbContext.SaveChangesAsync();


            var updatedBeneficiario = new Beneficiario
            {
                Id = id,
                Nome = "Silvana Araujo",
                Cpf = "32989238697",
                DataNascimento = new DateTime(1977, 01, 15),
                Email = "",
                Telefone = "33997690408",
                Cep = "39880972",
                Logradouro = "Rua Carneirinho Antonio Soares 157",
                Numero = "838",
                Complemento = "",
                Bairro = "Água Quente",
                Cidade = "Águas Formosas",
                Uf = "MG",
                situacao = api_web_service_bma.Enum.SituacaoEnum.Ativo,
                tipoCesta = api_web_service_bma.Enum.TipoCestaEnum.BASICA
            };

            //Act
            var result = await _controller.Update(id, updatedBeneficiario);

           //Assert
           //Ação concluída com êxito, mas não há conteúdo para retornar
            Assert.IsInstanceOf<NoContentResult>(result);

            //Busca o objeto atualizado no banco de dados
            //Com chave primária especificada
            var beneficiarioAtualizado = await _dbContext.Beneficiarios.FindAsync(id);

            Assert.That(beneficiarioAtualizado.Nome, Is.EqualTo(updatedBeneficiario.Nome));
            Assert.That(beneficiarioAtualizado.Cpf, Is.EqualTo(updatedBeneficiario.Cpf));
            Assert.That(beneficiarioAtualizado.DataNascimento, Is.EqualTo(updatedBeneficiario.DataNascimento));
            Assert.That(beneficiarioAtualizado.Email, Is.EqualTo(updatedBeneficiario.Email));

        }

        [Test]
        public async Task Delete()
        {

            //Arrange
            //Define os dados do teste
            int id = 10;

            var beneficiario = new Beneficiario
            {
                Id = id,
                Nome = "Pietra Ester Ferreira",
                Cpf = "32989238697",
                DataNascimento = new DateTime(1977, 01, 15),
                Email = "",
                Telefone = "33997690408",
                Cep = "39880972",
                Logradouro = "Rua Carneirinho Antonio Soares 157",
                Numero = "838",
                Complemento = "",
                Bairro = "Água Quente",
                Cidade = "Águas Formosas",
                Uf = "MG",
                situacao = api_web_service_bma.Enum.SituacaoEnum.Ativo,
                tipoCesta = api_web_service_bma.Enum.TipoCestaEnum.BASICA
            };

            await _dbContext.Beneficiarios.AddAsync(beneficiario);
            await _dbContext.SaveChangesAsync();

            //Act
            var result = await _controller.Delete(id);

            //Arrange
            //Ação concluída com êxito, mas não há conteúdo para retornar
            Assert.IsInstanceOf<NoContentResult>(result);
            
        }

    }
}