using System.Drawing;
using System.Net.Mime;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPdfDemo;

namespace QuestPdfDemo; 

public sealed class RelatorioGuia: IDocument {
    
    private readonly Guia _guia;
    private readonly string _pathToLogo;
    private readonly string _pathToFooterLogo;

    public RelatorioGuia(Guia guia, string pathToLogo, string pathToFooterLogo) {
        _guia = guia;
        _pathToLogo = pathToLogo;
        _pathToFooterLogo = pathToFooterLogo;
    }

    public void Compose(IDocumentContainer container) {
        container.Page(page => {
                           page.Size(PageSizes.A4);

                           TextStyle pageTextStyle = TextStyle.Default

                                                              .FontSize(12)
                                                              .FontColor(Colors.Black);
                           page.Margin(20);
                           page.DefaultTextStyle(pageTextStyle);
                           
                           page.Header( ).Element(ComposeHeader);
                           page.Content( ).Element(ComposeBody);
                           page.Footer( ).Element(ComposeFooter);
                       });
    }

    private void ComposeBody(IContainer container) {
        container.PaddingVertical(10)
                 .Column(col => {
                             col.Spacing(5);
                             
                             // row 2 for to
                             col.Item( ).Element(ComposeFrom);
                             
                             // row 3 for to
                             col.Item( ).Element(ComposeTo);

                             col.Item( ).Text($"Em {_guia.DataTransporte: dd/MM/yyyy hh:mm:ss}");

                             col.Item( ).Text("Equipamentos: ").FontColor(Colors.Red.Darken2);

                             col.Item( ).Element(ComposeTable);

                             col.Item( )
                                .AlignCenter( )
                                .PaddingTop(40)
                                .PaddingLeft(40)
                                .Text("Recebi em         /   /");
                         });
    }

    private void ComposeTo(IContainer container) {
        container.Row(row => {
                          row.RelativeItem(6).Text("");
                          row.RelativeItem().Column(col => {
                                                        col.Item( )
                                                           .AlignRight( )
                                                           .PaddingLeft(5)
                                                           .Text("Para:").SemiBold(  );
                                                    });
                          row.RelativeItem(5).Column(col => {
                                                         col.Item( )
                                                            .AlignLeft( )
                                                            .PaddingLeft(5)
                                                            .Text(x => {
                                                                      x.Line(_guia.LocalTrabalhoDestino);
                                                                      x.Line(_guia.FuncionarioDestino);
                                                                  });
                                                     });
                      });
    }

    private void ComposeFrom(IContainer container) {
        container.Row(row => {
                          row.RelativeItem(0.5F).Column(col => {
                                                            col.Item( )
                                                               .AlignRight( )
                                                               .Text("De").SemiBold(  );
                                                        });
                          row.RelativeItem(5.5F).Column(col => {
                                                            col.Item( )
                                                               .AlignLeft( )
                                                               .PaddingLeft(5)
                                                               .Text(x => {
                                                                         x.Line(_guia.LocalTrabalhoDestino);
                                                                         x.Line(_guia.FuncionarioOrigem);
                                                                     });
                                                        });
                      });
    }

    private void ComposeTable(IContainer container) {
        container.AlignCenter(  )
                 .PaddingTop(50)
                 .Table(table => {
                            table.ColumnsDefinition(cols => {
                                                        cols.RelativeColumn(); // tag
                                                        cols.RelativeColumn(2); // tipo
                                                        cols.RelativeColumn(2); // subtipo
                                                    });
                            
                            table.Header(header => {
                                             header.Cell( ).Element(CellStyle).AlignCenter(  ).Text("TAG");
                                             header.Cell( ).Element(CellStyle).AlignCenter(  ).Text("Tipo");
                                             header.Cell( ).Element(CellStyle).AlignCenter(  ).Text("Subtipo");
                                             
                                             static IContainer CellStyle(IContainer container) {
                                                 return container.DefaultTextStyle(x => x.SemiBold( ))
                                                                 .Padding(2)
                                                                 .BorderBottom(1)
                                                                 .BorderColor(Colors.Black);
                                             }
                                         });

                            foreach( ResumoEquipamento resumoEquipamento in _guia.Equipamentos ) {
                                table.Cell( ).Element(CellStyle).AlignCenter(  ).Text(resumoEquipamento.Tag);
                                table.Cell( ).Element(CellStyle).AlignLeft(  ).Text(resumoEquipamento.Tipo);
                                table.Cell( ).Element(CellStyle).AlignLeft(  ).Text(resumoEquipamento.Subtipo);
                            }
                            
                            static IContainer CellStyle(IContainer container) {
                                return container.PaddingVertical(2)
                                                .BorderBottom(1)
                                                .BorderColor(Colors.Grey.Lighten2);
                            }
                        });

        
    }

    private byte[] LoadLogoImage() {
        return File.ReadAllBytes(_pathToLogo);
    }

    private void ComposeHeader(IContainer container) {
        
        container.Column(c => {
                             // row 1 for title
                             c.Item(  ).PaddingVertical(10)
                              .Row(r => {
                                       //image
                                       r.ConstantItem(170).Image(LoadLogoImage());
                                       
                                       //text
                                       TextStyle headerTitleStyle = TextStyle.Default.FontSize(16)
                                                                             .SemiBold(  )
                                                                             .FontColor(Colors.Red.Darken2);
                                       r.RelativeItem( )
                                        .AlignRight(  )
                                        .PaddingTop(30)
                                        .Text($"Guia de Transporte nº: {_guia.IdGuia}")
                                        .Style(headerTitleStyle);


                                   });
                            
                             
                             
                             
                         });
        
    }

    private byte[] LoadFooterLogo() => File.ReadAllBytes(_pathToFooterLogo);
    
    private void ComposeFooter(IContainer container) {
        container.Padding(5)
                 .AlignCenter( )
                 .Height(60)
                 .Row(r => {
                          // footer left logo
                          r.RelativeItem(2).AlignCenter(  )
                                            .Height(50)
                                            .Image(LoadFooterLogo(  ))
                                            .FitHeight(  );
                          
                          // footer right side
                          r.RelativeItem(7).AlignCenter(  )
                           .DefaultTextStyle(TextStyle.Default.SemiBold())
                           .Column(c => {
                                       //page number row
                                       c.Item( ).AlignCenter( )
                                        .Text(x => {
                                                  x.Span("Pág. ");
                                                  x.CurrentPageNumber( );
                                                  x.Span(" de ");
                                                  x.TotalPages( );
                                              });
                                       
                                       // dri info row
                                       c.Item(  ).AlignCenter(  )
                                        .PaddingTop(3)
                                        .Row(r1 => {
                                                 // email
                                                 r1.RelativeItem( )
                                                   .PaddingRight(10)
                                                   .AlignRight( )
                                                   .Text("helpdesk.dri@madeira.gov.pt")
                                                   .FontColor(Colors.Blue.Darken1);
                                                 // phone
                                                 r1.RelativeItem( )
                                                   .AlignLeft( )
                                                   .PaddingRight(10)
                                                   .Text("TELF: 291 145 190")
                                                   .FontColor(Colors.Blue.Darken1);
                                             });
                                       
                                       // dri tec info
                                       c.Item( )
                                        .PaddingTop(3)
                                        .AlignCenter( )
                                        .Text($"Impresso por: {_guia.TecnicoImpressao} ({_guia.EmailTecnicoImpressao})");
                                   });
                          
                      });
                 /*
        
        
        container.BorderTop(1).Padding(5).BorderColor(Colors.Black)
                 .AlignCenter( )
                 .Height(60)
                 .Text(x => {
                           TextStyle footerStyle = TextStyle.Default.FontSize(10).Light( );
                           x.DefaultTextStyle(footerStyle);
                           x.AlignCenter( );
                           x.Span($"Impresso em {DateTime.Now:dd/MM/yyyy hh:mm:ss} por: ");
                           x.Span(_guia.TecnicoImpressao).SemiBold( );
                           x.EmptyLine( );

                           x.Span("Página ");
                           x.CurrentPageNumber( ).SemiBold( );
                           x.Span(" / ");
                           x.TotalPages( );
                       });*/
    }
}
