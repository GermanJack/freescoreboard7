/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Datetime from 'react-datetime';
import moment from 'moment';
import {
  IoIosSwap,
  IoIosShuffle,
  IoMdArrowDown,
  IoMdArrowUp,
  IoMdTrash,
} from 'react-icons/io';
import { VscLocation } from 'react-icons/vsc';
import { MdSchedule } from 'react-icons/md';

import Table from '../../StdComponents/Table';

import ClsTurnier from '../dataClasses/ClsTurnier';
import TSpiele from '../dataClasses/TSpiele';
import './DateTimePic.css';

import {
  multiSort,
} from '../SystemFunctions';

class Turnier3 extends Component {
  constructor() {
    super();
    this.state = {
      selSpiele: [],
      panel: 0,
      moment: moment(),
      interval: 20,
      spielort: '',
    };
  }

  // eslint-disable-next-line no-unused-vars
  componentDidUpdate(prevProps, prevState) {
    if (this.props.panel !== this.state.panel) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ panel: this.props.panel });
    }

    if (this.props.panel !== this.state.panel) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ panel: this.props.panel });
    }
  }

  spielRein(runde, gruppe, grpSpiel, mannschaftA, mannschaftB, sPlatz, vPlatz) {
    if (mannschaftA === '' || mannschaftB === '') {
      return;
    }

    const spielnr = this.props.turnier.spiele.length + 1;
    const spiel = new TSpiele();
    spiel.ID = this.props.turnier.spiele.length;
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

  selAllClick(e) {
    if (!e.target.checked) {
      this.setState({ selSpiele: [] });
    }

    if (e.target.checked) {
      this.setState({ selSpiele: this.props.turnier.Spiele.map((x) => x.ID) });
    }
  }

  grpMix() {
    const sl = this.props.turnier.Spiele;
    const sls = multiSort(sl, {
      Runde: 'asc',
      GruppenSpiel: 'asc',
      Gruppe: 'asc',
    });

    for (let i = 0; i < sls.length; i += 1) {
      sls[i].Spiel = i + 1;
    }
    // this.props.setDaten('spiele', sls);
  }

  switchTeams() {
    const sl = this.props.turnier.Spiele;
    for (let i = 0; i < this.state.selSpiele.length; i += 1) {
      const sInd = sl.findIndex((x) => x.ID === this.state.selSpiele[i]);
      const ma = sl[sInd].IstMannA;
      sl[sInd].IstMannA = sl[sInd].IstMannB;
      sl[sInd].PlanMannA = sl[sInd].IstMannB;
      sl[sInd].IstMannB = ma;
      sl[sInd].PlanMannB = ma;
    }
    // this.props.setDaten('spiele', sl);
  }

  spieleHoch() {
    if (this.state.selSpiele.length === 0) {
      return;
    }

    let sl = this.props.turnier.Spiele;
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
    // this.props.setDaten('spiele', sl);
  }

  spieleRunter() {
    if (this.state.selSpiele.length === 0) {
      return;
    }

    let sl = this.props.turnier.Spiele;
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
    // this.props.setDaten('spiele', sl);
  }

  spielDelete() {
    const sl = this.props.turnier.Spiele;
    for (let i = 0; i < this.state.selSpiele.length; i += 1) {
      const ind = sl.findIndex((x) => x.ID === this.state.selSpiele[i]);
      sl.splice(ind, 1);
    }
    // this.props.setDaten('spiele', sl);
  }

  handleMomentChange(e) {
    this.setState({ moment: e });
  }

  handleIntervalChange(e) {
    this.setState({ interval: e.target.value });
  }

  handleSpielortChange(e) {
    this.setState({ spielort: e.target.value });
  }

  terminierungRein() {
    const sl = this.props.turnier.Spiele;
    const m = moment(this.state.moment);
    for (let i = 0; i < this.state.selSpiele.length; i += 1) {
      const sInd = sl.findIndex((x) => x.ID === this.state.selSpiele[i]);
      if (i === 0) {
        sl[sInd].Uhrzeit = m.format('HH:mm');
      } else {
        sl[sInd].Uhrzeit = m.add(this.state.interval, 'minutes').format('HH:mm');
      }
      sl[sInd].Datum = m.format('DD.MMM.YYYY');
    }
    this.setState((state) => ({ ...state, render: !state.render }));
  }

  spielOrtRein() {
    const sl = this.props.turnier.Spiele;
    for (let i = 0; i < this.state.selSpiele.length; i += 1) {
      const sInd = sl.findIndex((x) => x.ID === this.state.selSpiele[i]);
      sl[sInd].Ort = this.state.spielort;
    }
    this.setState((state) => ({ ...state, render: !state.render }));
  }

  render() {
    const head = (
      <div>
        {'Grunddaten > Mannschaften > '}
        <b>Spiele</b>
      </div>
    );

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
                value={this.props.turnier.Kopf.Beschreibung}
                spellCheck="false"
                readOnly
              />
            </div>
          </div>

          <div className="form-group row mt-2">
            <div className="col-sm-3">
              Terminierung:
            </div>
            <div className="col-sm-4">
              <Datetime
                id="date"
                timeFormat="HH:mm"
                dateFormat="DD.MMM.YYYY"
                value={this.state.moment}
                onChange={this.handleMomentChange.bind(this)}
              />
            </div>
            <div className="mr-2 ml-1">
              Interval:
            </div>
            <input
              type="number"
              value={this.state.interval}
              onChange={this.handleIntervalChange.bind(this)}
              className="form-control form-control-sm"
              style={{ width: '60px' }}
            />
            <div className="mr-2 ml-2">
              Minuten
            </div>
            <button
              type="button"
              className="btn btn-primary border-dark m-1 mt-0"
              onClick={this.terminierungRein.bind(this)}
              title="Terminierung einfügen"
            >
              <MdSchedule size="1.2em" />
            </button>
          </div>

          <div className="d-flex flex-row mt-2">
            <div className="col-sm-3">
              Spielort:
            </div>
            <div className="col-sm-4">
              <input
                type="text"
                value={this.state.spielort}
                onChange={this.handleSpielortChange.bind(this)}
                className="form-control form-control-sm"
              />
            </div>
            <button
              type="button"
              className="btn btn-primary border-dark m-1 mt-0"
              onClick={this.spielOrtRein.bind(this)}
              title="Spielort einfügen"
            >
              <VscLocation size="1.2em" />
            </button>
          </div>
        </div>

        <div className="d-flex flex-row">
          <Table
            daten={this.props.turnier.Spiele}
            cols={[
              /* { Column: 'ID', Label: 'ID' }, */
              { Column: 'Spiel', Label: 'SpielNr' },
              { Column: 'Datum', Label: 'Datum' },
              { Column: 'Uhrzeit', Label: 'Uhrzeit' },
              { Column: 'Ort', Label: 'Spielort' },
              { Column: 'Runde', Label: 'Runde' },
              /* { Column: 'GruppenSpiel', Label: 'GrpSp' }, */
              { Column: 'Gruppe', Label: 'Gruppe' },
              { Column: 'IstMannA', Label: 'MannschaftA' },
              { Column: 'IstMannB', Label: 'MannschaftB' },
              { Column: 'SPlatz', Label: 'Siegerplatz' },
              { Column: 'VPlatz', Label: 'Verliererplatz' },
            ]}
            chkid={this.state.selSpiele}
            chk="Selected"
            chktype="checkbox"
            radiogrp="spiele"
            height="33vh"
            rowClick={this.rowClick.bind(this)}
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

Turnier3.propTypes = {
  turnier: PropTypes.instanceOf(ClsTurnier),
  clickWeiter: PropTypes.func,
  setDaten: PropTypes.func,
  panel: PropTypes.number,
};

Turnier3.defaultProps = {
  turnier: new ClsTurnier(),
  clickWeiter: () => {},
  setDaten: () => {},
  panel: 0,
};

export default Turnier3;
