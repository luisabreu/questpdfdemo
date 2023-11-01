using QuestPDF.Previewer;
using QuestPdfDemo;

Guia guia = new( ) {
                       IdGuia = 1,
                       LocalTrabalhoOrigem = "DRI - Dri",
                       FuncionarioOrigem = "Luis Miguel Nunes Abreu",
                       LocalTrabalhoDestino = "SRF - Secretaria Regional Finanças",
                       FuncionarioDestino = "Luis Abreu",
                       TecnicoImpressao = "Luis Miguel Abreu",
                       Equipamentos = new[] {
                                                new ResumoEquipamento {
                                                                          Subtipo = "Tablet",
                                                                          Tag = "tag0001212",
                                                                          Tipo = "Computador"
                                                                      },
                                                new ResumoEquipamento {
                                                                          Subtipo = "Ratos",
                                                                          Tag = "tag00012134",
                                                                          Tipo = "Rato"
                                                                      },
                                                new ResumoEquipamento {
                                                                          Subtipo = "Oticos",
                                                                          Tag = "tag00012135",
                                                                          Tipo = "Rato"
                                                                      }
                                            }
                   };

RelatorioGuia relatorio = new(guia);
relatorio.ShowInPreviewer();
