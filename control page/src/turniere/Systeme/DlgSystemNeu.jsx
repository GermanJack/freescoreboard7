import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { AiOutlineFileAdd } from 'react-icons/ai';
import System1 from './System1';
import System2 from './System2';
import System3 from './System3';
import System4 from './System4';

import TTabellen from '../dataClasses/TTabellen';
import TKopf from '../dataClasses/TKopf';
import ClsTurnier from '../dataClasses/ClsTurnier';

class DlgSystemNeu extends Component {
  constructor() {
    super();
    this.state = {
      panel: 1,
      SystemKopf: new TKopf(),
      runden: [],
      gruppen: [],
      spiele: [],
      tabellen: [],
    };
  }

  setDaten(e, v) {
    if (e === 'Kopf') {
      this.setState({ SystemKopf: v });
    }

    if (e === 'runde') {
      // eslint-disable-next-line react/no-access-state-in-setstate
      const r = this.state.runden;
      r.push(v);
      this.setState({ runden: r });
    }

    if (e === 'rundeRaus') {
      // eslint-disable-next-line react/no-access-state-in-setstate
      const r = this.state.runden;
      r.pop();
      this.setState({ runden: r });
    }

    if (e === 'rundenLeer') {
      this.setState({ runden: [] });
    }

    if (e === 'tabellen') {
      this.setState({ tabellen: v });
    }

    if (e === 'gruppen') {
      this.setState({ gruppen: v });
    }

    if (e === 'spiele') {
      this.setState({ spiele: v });
    }

    if (e === 'spiel+') {
      // eslint-disable-next-line react/no-access-state-in-setstate
      const x = this.state.spiele;
      x.push(v);
      this.setState({ spiele: x });
    }
  }

  clickWeiter(e) {
    let adder = e;

    if (adder > 0 && this.state.panel === 2) {
      // eslint-disable-next-line react/no-access-state-in-setstate
      this.setState({ tabellen: this.Tabellenerrechnen(this.state.runden) });
    }

    if (this.state.panel === 2 && adder > 0 && this.state.runden.length === 1) {
      adder += 1;
    }

    if (this.state.panel === 4 && adder < 0 && this.state.runden.length === 1) {
      adder -= 1;
    }

    // eslint-disable-next-line react/no-access-state-in-setstate
    const ns = this.state.panel + adder;
    this.setState({ panel: ns });
  }

  Tabellenerrechnen(runden) {
    // Mannschaften je Runde errechnen und in ds speichern
    const tab = [];

    for (let k = 0; k < runden.length; k += 1) {
      // anz grp
      for (let i = 1; i <= runden[k].AnzGrp; i += 1) {
        // anz mannsch je gruppe
        const mannschaftenanz = runden[k].AnzMann; // AnzGrp;

        for (let g = 1; g <= mannschaftenanz; g += 1) {
          const t = new TTabellen();
          t.ID = tab.length;
          t.Runde = runden[k].Runde;
          t.Gruppe = `${runden[k].Runde}${String.fromCharCode(64 + i)}`;
          t.Mannschaft = '';
          t.Spiele = 0;
          tab.push(t);
        }
      }
    }

    // Mannschaften der runde1 eingruppieren
    const r1 = tab.filter((x) => x.Runde === 1);

    let oldgrp = '';
    let mann = '';
    let m = 1;
    for (let i = 0; i < r1.length; i += 1) {
      if (r1[i].Gruppe !== oldgrp) {
        m = 1;
      }

      mann = `Mannschaft${m}_${r1[i].Gruppe}`;
      r1[i].Mannschaft = mann;
      oldgrp = r1[i].Gruppe;
      m += 1;
    }

    return tab;
  }

  handleClick() {
    const sys = new ClsTurnier(
      this.state.SystemKopf,
      this.state.runden,
      this.state.gruppen,
      this.state.tabellen,
      this.state.spiele,
    );

    const sysjson = JSON.stringify(sys);
    this.props.websocketSend({ Type: 'bef', Command: 'NewSystem', Value1: sysjson });

    this.setState({ panel: 1 });
    this.setState({ SystemKopf: '' });
    this.setState({ runden: [] });
    this.setState({ gruppen: [] });
    this.setState({ spiele: [] });
    this.setState({ tabellen: [] });
  }

  render() {
    const cn = this.props.btnclassname ? this.props.btnclassname : 'btn btn-primary btn-icon border-dark p-0 mr-1';
    let defaulticon = '';
    if (this.props.icon !== '') {
      defaulticon = (
        <AiOutlineFileAdd />
      );
    }

    const ico = this.props.icon ? defaulticon : this.props.reacticon;

    let pane = '';
    if (this.state.panel === 1) {
      pane = (
        <System1
          clickWeiter={this.clickWeiter.bind(this)}
          namensliste={this.props.namensliste}
          runden={this.state.runden}
          setDaten={this.setDaten.bind(this)}
          SystemKopf={this.state.SystemKopf}
        />
      );
    }
    if (this.state.panel === 2) {
      pane = (
        <System2
          clickWeiter={this.clickWeiter.bind(this)}
          setDaten={this.setDaten.bind(this)}
          runden={this.state.runden}
          SystemKopf={this.state.SystemKopf}
        />
      );
    }
    if (this.state.panel === 3) {
      pane = (
        <System3
          clickWeiter={this.clickWeiter.bind(this)}
          setDaten={this.setDaten.bind(this)}
          SystemKopf={this.state.SystemKopf}
          runden={this.state.runden}
          gruppen={this.state.gruppen}
          tabellen={this.state.tabellen}
          panel={this.state.panel}
        />
      );
    }
    if (this.state.panel === 4) {
      pane = (
        <System4
          clickWeiter={this.clickWeiter.bind(this)}
          setDaten={this.setDaten.bind(this)}
          SystemKopf={this.state.SystemKopf}
          runden={this.state.runden}
          gruppen={this.state.gruppen}
          tabellen={this.state.tabellen}
          spiele={this.state.spiele}
          panel={this.state.panel}
        />
      );
    }

    let disableSave = true;
    if (this.state.spiele.length > 0) {
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
                <h5 className="modal-title text-dark">neues Turniersystem</h5>
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

DlgSystemNeu.propTypes = {
  modalID: PropTypes.string,
  toolTip: PropTypes.string,
  reacticon: PropTypes.string,
  btnclassname: PropTypes.string,
  icon: PropTypes.string,
  namensliste: PropTypes.arrayOf(PropTypes.string),
  websocketSend: PropTypes.func,
};

DlgSystemNeu.defaultProps = {
  modalID: 'filedlg',
  toolTip: '',
  reacticon: '',
  btnclassname: '',
  icon: '',
  namensliste: [],
  websocketSend: () => {},
};

export default DlgSystemNeu;
