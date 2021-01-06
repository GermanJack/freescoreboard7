export default class TKopf {
  constructor(
    _ID,
    _TurnierNr,
    _Beschreibung,
    _Liga,
    _Kommentar,
    _Matrix,
    _Platzierungstyp,
    _Vorkommnisse,
    _Mananz,
    _status,
    _Version,
    _Turniertyp,
    _MatrixID,
  ) {
    this.ID = _ID;
    this.TurnierNr = _TurnierNr;
    this.Beschreibung = _Beschreibung;
    this.Liga = _Liga;
    this.Kommentar = _Kommentar;
    this.Matrix = _Matrix;
    this.Platzierungstyp = _Platzierungstyp;
    this.Vorkommnisse = _Vorkommnisse;
    this.Mananz = _Mananz;
    this.status = _status;
    this.Version = _Version;
    this.Turniertyp = _Turniertyp;
    this.MatrixID = _MatrixID;
  }
}
