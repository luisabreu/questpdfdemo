namespace QuestPdfDemo; 

public sealed class Guia {

    public int IdGuia { get; set; }

    public string FuncionarioOrigem { get; set; } = "";

    public string LocalTrabalhoOrigem { get; set; } = "";
    
    public string FuncionarioDestino { get; set; } = "";

    public string LocalTrabalhoDestino { get; set; } = "";

    public IEnumerable<ResumoEquipamento> Equipamentos { get; set; } = Enumerable.Empty<ResumoEquipamento>( );

    public string TecnicoImpressao { get; set; } = "";
}

public sealed class ResumoEquipamento {
    
    public string Tag { get; set; } = "";

    public string Tipo { get; set; } = "";

    public string Subtipo { get; set; } = "";
}
