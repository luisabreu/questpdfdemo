using QuestPDF.Fluent;
using QuestPDF.Previewer;
using QuestPdfDemo;

Guia guia = new( ) {
                       IdGuia = 1,
                       LocalTrabalhoOrigem = "DRI - Dri",
                       FuncionarioOrigem = "Luis Miguel Nunes Abreu",
                       LocalTrabalhoDestino = "SRF - Secretaria Regional Finanças",
                       FuncionarioDestino = "Luis Abreu",
                       TecnicoImpressao = "Luis Miguel Abreu",
                       EmailTecnicoImpressao = "luis.abreu@madeira.gov.pt",
                       DataTransporte = DateTime.Now,
                       Equipamentos = Enumerable.Range(0, 40)
                                                .Select(pos => new ResumoEquipamento {
                                                                                         Subtipo = "Tablet",
                                                                                         Tag = $"tag000121{pos}",
                                                                                         Tipo = "Computador"
                                                                                     })
                                                .ToList(  )
                   };


RelatorioGuia relatorio = new(guia);
Document.Merge(relatorio, relatorio)
        .UseOriginalPageNumbers(  )
        .ShowInPreviewer();
//relatorio.ShowInPreviewer();
