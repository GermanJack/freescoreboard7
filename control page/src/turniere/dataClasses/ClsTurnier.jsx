import TKopf from './TKopf';
import TRunden from './TRunden';
import TGruppen from './TGruppen';
import TTabellen from './TTabellen';
import TSpiele from './TSpiele';

export default class ClsTurnier {
  constructor(
    _Kopf = new TKopf(),
    _Runden = new TRunden(),
    _Gruppen = new TGruppen(),
    _Tabellen = new TTabellen(),
    _Spiele = new TSpiele(),
  ) {
    this.Kopf = _Kopf;
    this.Runden = _Runden;
    this.Gruppen = _Gruppen;
    this.Tabellen = _Tabellen;
    this.Spiele = _Spiele;
  }
}
