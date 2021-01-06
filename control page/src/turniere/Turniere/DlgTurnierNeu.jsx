import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { AiOutlineFileAdd } from 'react-icons/ai';
import Turnier1 from './Turnier1';
import Turnier2 from './Turnier2';
import Turnier3 from './Turnier3';

import ClsTurnier from '../dataClasses/ClsTurnier';
import TGruppen from '../dataClasses/TGruppen';
import TRunden from '../dataClasses/TRunden';
import TSpiele from '../dataClasses/TSpiele';

import { JGJgruppe } from '../SystemFunctions';

class DlgTurnierNeu extends Component {
  constructor() {
    super();
    this.state = {
      panel: 1,
      turnier: new ClsTurnier(),
    };
  }

  setDaten(e, v) {
    const t = v;
    if (e === 'turnier') {
      if (this.state.panel === 1) {
        if (v.Kopf.MatrixID === 0) {
          t.Runden = [new TRunden()];
          t.Runden[0].Runde = 1;
          t.Runden[0].status = 0;
          t.Gruppen = [new TGruppen()];
          t.Gruppen[0].Gruppe = '1A';
          t.Gruppen[0].status = 0;
          t.Spiele = [];
          t.Tabellen = [];
        } else {
          const s = this.props.systeme.filter((x) => x.Kopf.ID === v.Kopf.MatrixID);
          t.Runden = s[0].Runden;
          t.Gruppen = s[0].Gruppen;
          t.Spiele = s[0].Spiele;
          t.Tabellen = s[0].Tabellen;
          for (let i = 0; i < t.Tabellen.length; i += 1) {
            if (t.Tabellen[i].Runde === 1) {
              t.Tabellen[i].Mannschaft = '';
            }
          }
        }
      }
      this.setState({ turnier: t });
    }
  }

  ini() {
    this.setState({ panel: 1 });
    this.setState({ turnier: new ClsTurnier() });
  }

  clickWeiter(e) {
    const adder = e;

    if (adder > 0 && this.state.panel === 2 && this.state.turnier.Runden.length === 1) {
      if (this.state.turnier.Kopf.MatrixID === 0) {
        this.jgjErrechnen();
      }
    }

    // eslint-disable-next-line react/no-access-state-in-setstate
    const ns = this.state.panel + adder;

    if (ns === 3 && this.state.turnier.Kopf.MatrixID !== 0) {
      this.Spieleerrechnen();
    }

    this.setState({ panel: ns });
  }

  Spieleerrechnen() {
    // Spielplan generieren
    for (let i = 0; i < this.state.turnier.Spiele.length; i += 1) {
      const spiel = this.state.turnier.Spiele[i];
      spiel.Datum = '';
      spiel.Uhrzeit = '';
      spiel.Ort = '';

      if (spiel.Runde === 1) {
        spiel.IstMannA = this.GetMannschaft(spiel.PlanMannA);
        spiel.IstMannB = this.GetMannschaft(spiel.PlanMannB);
        spiel.Status = 2;
      }
    }
  }

  jgjErrechnen() {
    // JgJ Gruppe
    let Plan = [];
    const ml = this.state.turnier.Tabellen.filter(
      (x) => x.Runde === 1
      && x.Gruppe === '1A',
    ).map((y) => y.Mannschaft);
    Plan = JGJgruppe(1, '1A', ml);
    for (let i = 0; i < Plan.length; i += 1) {
      // not switched
      this.spielRein(
        Plan[i].Runde,
        Plan[i].Gruppe,
        Plan[i].Spiel,
        Plan[i].MannA,
        Plan[i].MannB,
        0,
        0,
      );
    }
  }

  spielRein(runde, gruppe, grpSpiel, mannschaftA, mannschaftB, sPlatz, vPlatz) {
    if (mannschaftA === '' || mannschaftB === '') {
      return;
    }

    const t = this.state.turnier;

    const spielnr = this.state.turnier.Spiele.length + 1;
    const spiel = new TSpiele();
    spiel.ID = this.state.turnier.Spiele.length;
    spiel.Spiel = spielnr;
    spiel.Datum = '';
    spiel.Uhrzeit = '';
    spiel.Ort = '';
    spiel.Runde = runde;
    spiel.Gruppe = gruppe;
    spiel.GruppenSpiel = grpSpiel;
    spiel.IstMannA = mannschaftA;
    spiel.PlanMannA = mannschaftA;
    spiel.IstMannB = mannschaftB;
    spiel.PlanMannB = mannschaftB;
    spiel.SPlatz = sPlatz;
    spiel.VPlatz = vPlatz;
    spiel.status = 2;

    t.Spiele.push(spiel);

    this.setDaten('turnier', t);
  }

  GetMannschaft(planMannschaft) {
    // suche erste Zahl
    let start = 0;
    for (let i = 1; i < planMannschaft.length; i += 1) {
      const c = planMannschaft.charCodeAt(i);
      if (c >= 48 && c <= 57) {
        start = i;
        break;
      }
    }

    // abschneiden
    const ManNrGr = planMannschaft.substring(start, planMannschaft.length);

    // nr und gruppe trennen
    const nrgrarr = ManNrGr.split('_');

    // auslesen
    const gruppe = nrgrarr[1];
    const ManNr = parseInt(nrgrarr[0], 10);

    const grp = this.state.turnier.Tabellen.filter((x) => x.Gruppe === gruppe);

    return grp[ManNr - 1].Mannschaft;
  }

  handleClick() {
    const sysjson = JSON.stringify(this.state.turnier);
    this.props.websocketSend({ Type: 'bef', Command: 'NewTurnier', Value1: sysjson });
    this.ini();
  }

  render() {
    const cn = this.props.btnclassname ? this.props.btnclassname : 'btn btn-primary btn-icon border-dark p-0 mr-1';
    let icon = '';
    if (this.props.icon !== '') {
      icon = (
        <AiOutlineFileAdd size="1.2em" />
      );
    }

    const ico = this.props.icon ? icon : this.props.reacticon;

    let pane = '';
    if (this.state.panel === 1) {
      pane = (
        <Turnier1
          clickWeiter={this.clickWeiter.bind(this)}
          namensliste={this.props.namensliste}
          turnier={this.state.turnier}
          setDaten={this.setDaten.bind(this)}
          systeme={this.props.systeme}
          teamList={this.props.teamList}
        />
      );
    }
    if (this.state.panel === 2) {
      pane = (
        <Turnier2
          clickWeiter={this.clickWeiter.bind(this)}
          setDaten={this.setDaten.bind(this)}
          turnier={this.state.turnier}
          teamList={this.props.teamList}
        />
      );
    }
    if (this.state.panel === 3) {
      pane = (
        <Turnier3
          turnier={this.state.turnier}
          clickWeiter={this.clickWeiter.bind(this)}
          setDaten={this.setDaten.bind(this)}
          teamList={this.props.teamList}
          panel={this.state.panel}
        />
      );
    }

    let disableSave = true;
    if (this.state.turnier.Spiele.length > 0 && this.state.panel === 3) {
      disableSave = false;
    }

    return (
      <div className="d-flex flex-column">
        <button
          type="button"
          className={cn}
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
        >
          {ico}
        </button>

        <div className="modal fade" id={this.props.modalID} tabIndex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
          <div className="modal-dialog modal-lg">
            <div className="modal-content">

              <div className="modal-header">
                <h5 className="modal-title text-dark">neues Turnier</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">
                    &times;
                  </span>
                </button>
              </div>

              <div className="modal-body">
                <div className="d-flex flex-column">
                  {pane}
                </div>
              </div>

              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-primary save"
                  disabled={disableSave}
                  onClick={this.handleClick.bind(this)}
                  data-dismiss="modal"
                >
                  Speichern
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgTurnierNeu.propTypes = {
  modalID: PropTypes.string,
  toolTip: PropTypes.string,
  reacticon: PropTypes.string,
  btnclassname: PropTypes.string,
  icon: PropTypes.string,
  namensliste: PropTypes.arrayOf(PropTypes.string),
  systeme: PropTypes.arrayOf(PropTypes.object),
  teamList: PropTypes.arrayOf(PropTypes.object),
  websocketSend: PropTypes.func,
};

DlgTurnierNeu.defaultProps = {
  modalID: 'filedlg',
  toolTip: '',
  reacticon: '',
  btnclassname: '',
  icon: '',
  namensliste: [],
  systeme: [],
  teamList: [],
  websocketSend: () => {},
};

export default DlgTurnierNeu;
