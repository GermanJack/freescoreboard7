export default class TEreignisse {
  constructor(
    _ID,
    _TurnierNr,
    _Spiel,
    _Mannschaft,
    _Spielzeit,
    _Spielzeitrichtung,
    _CPUZeit,
    _Ereignistyp,
    _Spieler,
    _Details,
    _Spielabschnitt = 1,
    _TurnierID,
  ) {
    this.ID = _ID;
    this.TurnierNr = _TurnierNr;
    this.Spiel = _Spiel;
    this.Mannschaft = _Mannschaft;
    this.Spielzeit = _Spielzeit;
    this.Spielzeitrichtung = _Spielzeitrichtung;
    this.CPUZeit = _CPUZeit;
    this.Ereignistyp = _Ereignistyp;
    this.Spieler = _Spieler;
    this.Details = _Details;
    this.Spielabschnitt = _Spielabschnitt;
    this.TurnierID = _TurnierID;
  }
}
