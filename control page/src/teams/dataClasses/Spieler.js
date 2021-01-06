export default class Spieler {
  constructor(
    _ID,
    _MannschaftsID,
    _Nachname,
    _Vorname,
    _SID,
    _NickName,
    _Kurzname,
    _Bild,
  ) {
    this.ID = _ID;
    this.MannschaftsID = _MannschaftsID;
    this.Nachname = _Nachname;
    this.Vorname = _Vorname;
    this.SID = _SID;
    this.NickName = _NickName;
    this.Kurzname = _Kurzname;
    this.Bild = _Bild;
  }
}
