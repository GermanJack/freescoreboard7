export default class ClsQuellPlaetze {
  constructor(
    _ID = 0,
    _Quelltyp = '',
    _QuellRunde = 0,
    _QuellGruppe = '',
    _QuellGruppenplatz = 0,
    _QuellRundenplatz = 0,
    _Text = '',
  ) {
    this.ID = _ID;
    this.Quelltyp = _Quelltyp;
    this.QuellRunde = _QuellRunde;
    this.QuellGruppe = _QuellGruppe;
    this.QuellGruppenplatz = _QuellGruppenplatz;
    this.QuellRundenplatz = _QuellRundenplatz;
    this.Text = _Text;
  }
}
