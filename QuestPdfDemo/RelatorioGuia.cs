using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPdfDemo;

namespace QuestPdfDemo; 

public sealed class RelatorioGuia: IDocument {
    
    private readonly Guia _guia;

    public RelatorioGuia(Guia guia) {
        _guia = guia;
    }

    public void Compose(IDocumentContainer container) {
        container.Page(page => {
                           page.Size(PageSizes.A4);
            
                           page.Margin(10);
                           
                           page.Header( ).Element(ComposeHeader);
                           page.Content( ).Element(ComposeBody);
                           page.Footer( ).Element(ComposeFooter);
                       });
    }

    private void ComposeBody(IContainer container) {
        container.PaddingVertical(10)
                 .Column(col => {
                             col.Spacing(5);

                             col.Item( ).Element(ComposeTable);
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

    private void ComposeHeader(IContainer container) {
        TextStyle titleStyle = TextStyle.Default.FontSize(25).SemiBold( );
        
        container.Column(c => {
                             // row 1 for title
                             c.Item(  ).PaddingTop(10)
                              .AlignCenter(  )
                              .Text($"Guia {_guia.IdGuia}").Style(titleStyle);
                             
                             // row 2 for to/from
                             c.Item(  ).PaddingTop(10)
                              .Row(row => {
                                       row.RelativeItem().Column(col => {
                                                                     col.Item( )
                                                                        .AlignLeft( )
                                                                        .PaddingLeft(10)
                                                                        .Text(x => {
                                                                                  x.Span("De: ").SemiBold( );
                                                                                  x.Span(_guia.FuncionarioOrigem);
                                                                                  x.EmptyLine( );
                                                                                  x.Span($"          {_guia.LocalTrabalhoOrigem}");
                                                                              });
                                                                 });
                                       row.RelativeItem().Column(col => {
                                                                     col.Item( )
                                                                        .AlignRight( )
                                                                        .PaddingTop(50)
                                                                        .Text(x => {
                                                                                  x.Span("Para: ").SemiBold( );
                                                                                  x.Span(_guia.FuncionarioDestino);
                                                                                  x.EmptyLine( );
                                                                                  x.Span($"             {_guia.LocalTrabalhoDestino}");
                                                                              });
                                                                 });             
                                   });
                         });
        
    }

    private void ComposeFooter(IContainer container) {
        
        container.BorderTop(1).Padding(5).BorderColor(Colors.Black)
                 .AlignCenter( )
                 .Height(40)
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
                       });
    }
}
