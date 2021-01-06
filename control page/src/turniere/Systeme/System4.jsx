/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import {
  IoIosSwap,
  IoIosShuffle,
  IoMdArrowDown,
  IoMdArrowUp,
  IoMdTrash,
  IoMdReturnLeft,
} from 'react-icons/io';
import { CgPlayListAdd } from 'react-icons/cg';

import Table from '../../StdComponents/Table';
import DropdownWithID from '../../StdComponents/DropdownWithID';
import Dropdown from '../../StdComponents/Dropdown';

import TKopf from '../dataClasses/TKopf';
import TRunden from '../dataClasses/TRunden';
import TSpiele from '../dataClasses/TSpiele';
import TTabellen from '../dataClasses/TTabellen';

import {
  JGJgruppe,
  jgjRunde,
  jgjTurnier,
  calcPlaetze,
  multiSort,
} from '../SystemFunctions';

class System4 extends Component {
  constructor() {
    super();
    this.state = {
      runde: '',
      gruppe: '',
      MannschaftA: '',
      MannschaftB: '',
      splatz: 0,
      vplatz: 0,

      platzList: [],

      reinType: 'A',

      selSpiele: [],
      selAllChecked: false,
    };
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.panel === 4 && this.state.panel !== 4) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ platzList: calcPlaetze(this.props.SystemKopf.Mananz, this.props.spiele) });
    }
    if (this.props.panel !== this.state.panel) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ panel: this.props.panel });
    }

    if (this.props.panel !== this.state.panel) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ panel: this.props.panel });
    }

    this.checkSelAllStates();
  }

  rundeChange(e) {
    this.setState({ runde: e });
  }

  gruppeChange(e) {
    this.setState({ gruppe: e });
  }

  manAChange(e) {
    this.setState({ MannschaftA: e });
  }

  manBChange(e) {
    this.setState({ MannschaftB: e });
  }

  sPlatzChange(e) {
    this.setState({ splatz: e });
  }

  vPlatzChange(e) {
    this.setState({ vplatz: e });
  }

  exeCommand() {
    const type = this.state.reinType;
    if (type === 'A') {
      this.spielRein(
        this.state.runde,
        this.state.gruppe,
        this.props.spiele.length + 1,
        this.state.MannschaftA,
        this.state.MannschaftB,
        this.state.splatz,
        this.state.vplatz,
      );
    }

    if (type === 'F' || type === 'G') {
      // JgJ Turnier
      let Plan = [];
      Plan = jgjTurnier(this.props.tabellen);
      for (let i = 0; i < Plan.length; i += 1) {
        if (type === 'F') {
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
        if (type === 'G') {
          // switched
          this.spielRein(
            Plan[i].Runde,
            Plan[i].Gruppe,
            Plan[i].Spiel,
            Plan[i].MannB,
            Plan[i].MannA,
            0,
            0,
          );
        }
      }
    }

    if (type === 'D' || type === 'E') {
      // JgJ Runde
      let Plan = [];
      const ml = this.props.tabellen.filter(
        (x) => x.Runde === this.state.runde,
      );
      const gl = this.props.tabellen.filter(
        (x) => x.Runde === this.state.runde,
      ).map((y) => y.Gruppe);
      const distinctGl = [...new Set(gl)];
      Plan = jgjRunde(this.state.runde, distinctGl, ml);
      for (let i = 0; i < Plan.length; i += 1) {
        if (type === 'D') {
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
        if (type === 'E') {
          // switched
          this.spielRein(
            Plan[i].Runde,
            Plan[i].Gruppe,
            Plan[i].Spiel,
            Plan[i].MannB,
            Plan[i].MannA,
            0,
            0,
          );
        }
      }
    }

    if (type === 'B' || type === 'C') {
      // JgJ Gruppe
      let Plan = [];
      const ml = this.props.tabellen.filter(
        (x) => x.Runde === this.state.runde
        && x.Gruppe === this.state.gruppe,
      ).map((y) => y.Mannschaft);
      Plan = JGJgruppe(this.state.runde, this.state.gruppe, ml);
      for (let i = 0; i < Plan.length; i += 1) {
        if (type === 'B') {
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
        if (type === 'C') {
          // switched
          this.spielRein(
            Plan[i].Runde,
            Plan[i].Gruppe,
            Plan[i].Spiel,
            Plan[i].MannB,
            Plan[i].MannA,
            0,
            0,
          );
        }
      }
    }

    this.ini();
  }

  ini() {
    this.setState({ runde: '' });
    this.setState({ gruppe: '' });
    this.setState({ MannschaftA: '' });
    this.setState({ MannschaftB: '' });
    this.setState({ splatz: 0 });
    this.setState({ vplatz: 0 });
  }

  spielRein(runde, gruppe, grpSpiel, mannschaftA, mannschaftB, sPlatz, vPlatz) {
    if (mannschaftA === '' || mannschaftB === '') {
      return;
    }

    const spielnr = this.props.spiele.length + 1;
    const spiel = new TSpiele();
    spiel.ID = this.props.spiele.length;
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

    this.props.setDaten('spiel+', spiel);
  }

  plaetzeRein() {

  }

  reinTypeChange(e) {
    this.setState({ reinType: e });
  }

  checkSelAllStates() {
    const hlp = this.state.selSpiele;
    if (hlp.length === 0) {
      if (this.state.selAllChecked !== false) {
        this.setState({ selAllChecked: false });
      }
    } else {
      if (hlp.length === this.props.spiele.length) {
        if (this.state.selAllChecked !== true) {
          this.setState({ selAllChecked: true });
        }
      }

      if (hlp.length > 0 && hlp.length < this.props.spiele.length) {
        if (this.state.selAllChecked !== false) {
          this.setState({ selAllChecked: false });
        }
      }
    }
  }

  rowClick(e) {
    // eslint-disable-next-line react/no-access-state-in-setstate
    const hlp = this.state.selSpiele;
    if (this.state.selSpiele.includes(e)) {
      const ind = hlp.findIndex((x) => x === e);
      hlp.splice(ind, 1);
    } else {
      hlp.push(e);
    }
    this.setState({ selSpiele: hlp });
  }

  selAllClick() {
    if (this.state.selAllChecked === true) {
      this.setState({ selSpiele: [] });
    }

    if (this.state.selAllChecked === false) {
      this.setState({ selSpiele: this.props.spiele.map((x) => x.ID) });
    }
  }

  addPlaetze() {
    if (this.state.selSpiele.length === 0) {
      return;
    }

    const sl = this.props.spiele;
    const ids = this.state.selSpiele.sort((a, b) => a - b);
    const sInd = sl.findIndex((x) => x.ID === ids[0]);
    sl[sInd].SPlatz = this.state.splatz;
    sl[sInd].VPlatz = this.state.vplatz;
    this.props.setDaten('spiele', sl);
  }

  grpMix() {
    const sl = this.props.spiele;
    const sls = multiSort(sl, {
      Runde: 'asc',
      GruppenSpiel: 'asc',
      Gruppe: 'asc',
    });

    for (let i = 0; i < sls.length; i += 1) {
      sls[i].Spiel = i + 1;
    }
    this.props.setDaten('spiele', sls);
  }

  switchTeams() {
    const sl = this.props.spiele;
    for (let i = 0; i < this.state.selSpiele.length; i += 1) {
      const sInd = sl.findIndex((x) => x.ID === this.state.selSpiele[i]);
      const ma = sl[sInd].IstMannA;
      sl[sInd].IstMannA = sl[sInd].IstMannB;
      sl[sInd].PlanMannA = sl[sInd].IstMannB;
      sl[sInd].IstMannB = ma;
      sl[sInd].PlanMannB = ma;
    }
    this.props.setDaten('spiele', sl);
  }

  spieleHoch() {
    if (this.state.selSpiele.length === 0) {
      return;
    }

    let sl = this.props.spiele;
    const ids = this.state.selSpiele.sort((a, b) => a - b);
    for (let i = 0; i < ids.length; i += 1) {
      const sInd = sl.findIndex((x) => x.ID === ids[i]);
      if (sInd === 0) {
        // return falls schon ganz oben
        return;
      }
      sl[sInd - 1].Spiel += 1;
      sl[sInd].Spiel -= 1;
      sl = multiSort(sl, { Spiel: 'asc' });
    }
    this.props.setDaten('spiele', sl);
  }

  spieleRunter() {
    if (this.state.selSpiele.length === 0) {
      return;
    }

    let sl = this.props.spiele;
    const ids = this.state.selSpiele.sort((a, b) => b - a);
    for (let i = 0; i < ids.length; i += 1) {
      const sInd = sl.findIndex((x) => x.ID === ids[i]);
      if (sInd === sl.length - 1) {
        // return falls schon ganz unten
        return;
      }
      sl[sInd + 1].Spiel -= 1;
      sl[sInd].Spiel += 1;
      sl = multiSort(sl, { Spiel: 'asc' });
    }
    this.props.setDaten('spiele', sl);
  }

  spielDelete() {
    const sl = this.props.spiele;
    for (let i = 0; i < this.state.selSpiele.length; i += 1) {
      const ind = sl.findIndex((x) => x.ID === this.state.selSpiele[i]);
      sl.splice(ind, 1);
    }
    this.props.setDaten('spiele', sl);
    this.setState({ selSpiele: [] });
    this.setState({ selAllChecked: false });
  }

  render() {
    const head = (
      <div>
        {'Grunddaten > Runden > Gruppen > '}
        <b>Spiele</b>
      </div>
    );

    const rund = this.props.runden.map((x) => x.Runde);
    const grup = this.props.tabellen.filter(
      (x) => x.Runde === this.state.runde,
    ).map((y) => y.Gruppe);
    const distinctGrup = [...new Set(grup)];

    const mann = this.props.tabellen.filter(
      (x) => x.Gruppe === this.state.gruppe,
    ).map((y) => y.Mannschaft);

    const message = '';

    let displayPlaetze = 'd-none';
    if (this.props.SystemKopf.Platzierungstyp === 'P') {
      displayPlaetze = 'mr-2';
    }

    return (
      <div className="d-flex flex-column">
        {head}

        <div className="">
          <div className="form-group row mt-1">
            <label htmlFor="name" className="col-sm-3 col-form-label">Name</label>
            <div className="col-sm-5">
              <input
                type="Text"
                className="form-control form-control-sm mt-1"
                id="name"
                value={this.props.SystemKopf.Beschreibung}
                spellCheck="false"
                readOnly
              />
            </div>
          </div>

          <div className="d-flex flex-row">
            <div className="mr-2">
              <label htmlFor="runde" className="col-form-label">Runden-Nr.</label>
              <div className="">
                <Dropdown
                  id="runde"
                  values={rund}
                  value={this.state.runde}
                  cName="form-control form-control-sm mt-1 text-left"
                  wahl={this.rundeChange.bind(this)}
                />
              </div>
            </div>
            <div className="mr-2">
              <label htmlFor="grp" className="col-form-label">Gruppen-Nr.</label>
              <div className="">
                <Dropdown
                  id="grp"
                  values={distinctGrup}
                  value={this.state.gruppe}
                  cName="form-control form-control-sm mt-1 text-left"
                  wahl={this.gruppeChange.bind(this)}
                />
              </div>
            </div>
            <div className="mr-2">
              <label htmlFor="manA" className="col-form-label">Mannschaft A</label>
              <div className="">
                <Dropdown
                  id="manA"
                  values={mann}
                  value={this.state.MannschaftA}
                  cName="form-control form-control-sm mt-1 text-left"
                  wahl={this.manAChange.bind(this)}
                />
              </div>
            </div>
            <div className="mr-2">
              <label htmlFor="manB" className="col-form-label">Mannschaft B</label>
              <div className="">
                <Dropdown
                  id="manB"
                  values={mann}
                  value={this.state.MannschaftB}
                  cName="form-control form-control-sm mt-1 text-left"
                  wahl={this.manBChange.bind(this)}
                />
              </div>
            </div>

            <div className={displayPlaetze}>
              <label htmlFor="spla" className="col-form-label">Siegerplatz</label>
              <div className="">
                <Dropdown
                  id="spla"
                  values={this.state.platzList}
                  value={this.state.splatz}
                  cName="form-control form-control-sm mt-1 text-left"
                  wahl={this.sPlatzChange.bind(this)}
                />
              </div>
            </div>
            <div className={displayPlaetze}>
              <label htmlFor="lpla" className="col-form-label">Verliererplatz</label>
              <div className="">
                <Dropdown
                  id="lpla"
                  values={this.state.platzList}
                  value={this.state.vplatz}
                  cName="form-control form-control-sm mt-1 text-left"
                  wahl={this.vPlatzChange.bind(this)}
                />
              </div>
            </div>
            <div className={displayPlaetze}>
              <label htmlFor="prein" className="col-form-label">Platzierung einfügen</label>
              <div className="">
                <button
                  id="prein"
                  type="button"
                  className="btn btn-primary border-dark m-1"
                  onClick={this.addPlaetze.bind(this)}
                  title="Platzierungen einfügen"
                >
                  <IoMdReturnLeft />
                </button>
              </div>
            </div>
          </div>
        </div>

        <div className="d-flex flex-row mb-1">
          {/* Einfügenkontrols oben */}
          <DropdownWithID
            id="jgj"
            values={[
              { ID: 'A', Text: 'manuell definiertes Spiel übernehmen' },
              { ID: 'B', Text: 'auto Jeder gegen Jeden für gewählte Gruppe A : B' },
              { ID: 'C', Text: 'auto Jeder gegen Jeden für gewählte Gruppe B : A' },
              { ID: 'D', Text: 'auto Jeder gegen Jeden für gewählte Runde A : B' },
              { ID: 'E', Text: 'auto Jeder gegen Jeden für gewählte Runde B : A' },
              { ID: 'F', Text: 'auto Jeder gegen Jeden für komplettes Turnier A : B' },
              { ID: 'G', Text: 'auto Jeder gegen Jeden für komplettes Turnier B : A' },
            ]}
            value={this.state.reinType}
            cName="form-control form-control-sm mt-1 dropdown-toggle text-left"
            wahl={this.reinTypeChange.bind(this)}
          />

          <button
            type="button"
            className="btn btn-primary border-dark m-1"
            onClick={this.exeCommand.bind(this)}
            title="Spiel in Spieleliste übernehmen"
          >
            <CgPlayListAdd size="1.2em" />
          </button>

          <div className="mr-1 pt-1">{message}</div>
        </div>

        <div className="d-flex flex-row">
          <Table
            daten={this.props.spiele}
            cols={[
              /* { Column: 'ID', Label: 'ID' }, */
              { Column: 'Spiel', Label: 'SpielNr' },
              { Column: 'Runde', Label: 'Runde' },
              /* { Column: 'GruppenSpiel', Label: 'GrpSp' }, */
              { Column: 'Gruppe', Label: 'Gruppe' },
              { Column: 'PlanMannA', Label: 'MannschaftA' },
              { Column: 'PlanMannB', Label: 'MannschaftB' },
              { Column: 'SPlatz', Label: 'Siegerplatz' },
              { Column: 'VPlatz', Label: 'Verliererplatz' },
            ]}
            chkid={this.state.selSpiele}
            chk="Selected"
            chktype="checkbox"
            radiogrp="spiele"
            height="33vh"
            rowClick={this.rowClick.bind(this)}
            selAllChecked={this.state.selAllChecked}
            selAllClick={this.selAllClick.bind(this)}
          />

          <div className="d-flex flex-column pt-4">
            {/* Bearbeitungsknöpfe rechts */}
            <button
              type="button"
              className="btn btn-primary border-dark m-1"
              onClick={this.grpMix.bind(this)}
              title="Spiele der Gruppen mixen"
            >
              <IoIosShuffle size="1.2em" />
            </button>

            <button
              type="button"
              className="btn btn-primary border-dark m-1"
              onClick={this.switchTeams.bind(this)}
              title="Mannschaft A und B tauschen"
            >
              <IoIosSwap size="1.2em" />
            </button>

            <button
              type="button"
              className="btn btn-primary border-dark m-1"
              onClick={this.spieleHoch.bind(this)}
              title="Spiel nach oben"
            >
              <IoMdArrowUp size="1.2em" />
            </button>

            <button
              type="button"
              className="btn btn-primary border-dark m-1"
              onClick={this.spieleRunter.bind(this)}
              title="Spiel nach unten"
            >
              <IoMdArrowDown size="1.2em" />
            </button>

            <button
              type="button"
              className="btn btn-primary border-dark m-1"
              onClick={this.spielDelete.bind(this)}
              title="Spiel löschen"
            >
              <IoMdTrash size="1.2em" />
            </button>
          </div>
        </div>

        <div className="d-flex flex-row justify-content-start">
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.props.clickWeiter.bind(this, -1)}>
            Zurück
          </button>
        </div>
      </div>
    );
  }
}

System4.propTypes = {
  SystemKopf: PropTypes.objectOf(TKopf),
  runden: PropTypes.arrayOf(PropTypes.TRunden),
  clickWeiter: PropTypes.func,
  setDaten: PropTypes.func,
  spiele: PropTypes.arrayOf(PropTypes.TSpiele),
  tabellen: PropTypes.arrayOf(PropTypes.TTabellen),
  panel: PropTypes.number,
};

System4.defaultProps = {
  SystemKopf: '',
  runden: [new TRunden()],
  clickWeiter: () => {},
  setDaten: () => {},
  spiele: [new TSpiele()],
  tabellen: [new TTabellen()],
  panel: 0,
};

export default System4;
