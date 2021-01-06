/* eslint-disable react/no-access-state-in-setstate */
/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Table from '../../StdComponents/Table';
import DropdownWithID from '../../StdComponents/DropdownWithID';

import TKopf from '../dataClasses/TKopf';

import { MannschaftenFuellen, GruppenFuellen } from '../SystemFunctions';

class System3 extends Component {
  constructor() {
    super();
    this.state = {
      runde: 2,
      man: '1',
      manlist: [],
      grplist: [],
      quellPlaetzeList: [],

      manID: [],
      grpID: [],
      panel: 0,

    };
  }

  componentDidUpdate(prevProps, prevState) {
    if (this.props.panel === 3 && this.state.panel !== 3) {
      if (this.state.quellPlaetzeList.length === 0) {
        this.MannschaftChange('1');
      }
      if (this.state.grplist.length === 0) {
        const gf = GruppenFuellen(this.props.tabellen, this.state.runde);
        // eslint-disable-next-line react/no-did-update-set-state
        this.setState({ grplist: gf });
      }
    }
    if (this.props.panel !== this.state.panel) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ panel: this.props.panel });
    }

    if (prevState.runde !== this.state.runde) {
      const ml = MannschaftenFuellen(
        this.state.man,
        this.props.runden,
        this.state.runde,
        this.props.tabellen,
      );
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ quellPlaetzeList: ml });

      const gl = GruppenFuellen(
        this.props.tabellen,
        this.state.runde,
      );
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ grplist: gl });
    }
  }

  rundeChange(e) {
    this.setState({ runde: e.target.value });

    const ml = MannschaftenFuellen(
      this.state.man,
      this.props.runden,
      e.target.value,
      this.props.tabellen,
    );

    this.setState({ quellPlaetzeList: ml });
    this.setState({ manID: [] });
    this.setState({ grpID: [] });
  }

  MannschaftChange(e) {
    this.setState({ man: e });

    const ml = MannschaftenFuellen(
      e,
      this.props.runden,
      this.state.runde,
      this.props.tabellen,
    );

    this.setState({ quellPlaetzeList: ml });
    this.setState({ manID: [] });
    this.setState({ grpID: [] });
  }

  rowClickMan(e) {
    const hlp = this.state.manID;
    if (this.state.manID.includes(e)) {
      const ind = hlp.findIndex((x) => x === e);
      hlp.splice(ind, 1);
    } else {
      hlp.push(e);
    }
    this.setState({ manID: hlp });
  }

  rowClickGrp(e) {
    const hlp = this.state.grpID;
    if (this.state.grpID.includes(e)) {
      const ind = hlp.findIndex((x) => x === e);
      hlp.splice(ind, 1);
    } else {
      hlp.push(e);
    }
    this.setState({ grpID: hlp });
  }

  Rein() {
    const rundenNr = parseInt(this.state.runde, 10);
    const t = this.props.tabellen;
    for (let i = 0; i < this.state.manID.length; i += 1) {
      const ind = t.findIndex((x) => x.Runde === rundenNr && x.Mannschaft === '');
      const ml = t[ind];

      const qp = this.state.quellPlaetzeList.find((x) => x.ID === this.state.manID[i]);
      if (qp != null) {
        ml.Mannschaft = qp.Text;
        ml.Quelltyp = qp.Quelltyp;
        ml.QuellGruppe = qp.QuellGruppe;
        ml.QuellGruppenplatz = qp.QuellGruppenplatz;
        ml.Quellrunde = qp.QuellRunde;
        ml.QuellRundenplatz = qp.QuellRundenplatz;
      }
      t[ind] = ml;
    }
    this.props.setDaten('tabellen', t);

    const mf = MannschaftenFuellen(
      this.state.man,
      this.props.runden,
      this.state.runde,
      this.props.tabellen,
    );
    this.setState({ quellPlaetzeList: mf });

    const gf = GruppenFuellen(this.props.tabellen, this.state.runde);
    this.setState({ grplist: gf });
    this.setState({ manID: [] });
  }

  Raus() {
    const rundenNr = parseInt(this.state.runde, 10);
    const t = this.props.tabellen;
    for (let i = 0; i < this.state.grpID.length; i += 1) {
      const ind = t.findIndex((x) => x.Runde === rundenNr && x.ID === this.state.grpID[i]);
      const ml = t[ind];

      ml.Mannschaft = '';
      ml.Quelltyp = '';
      ml.QuellGruppe = '';
      ml.QuellGruppenplatz = 0;
      ml.Quellrunde = 0;
      ml.QuellRundenplatz = 0;
      t[ind] = ml;
    }
    this.props.setDaten('tabellen', t);

    const mf = MannschaftenFuellen(
      this.state.man,
      this.props.runden,
      this.state.runde,
      this.props.tabellen,
    );
    this.setState({ quellPlaetzeList: mf });

    const gf = GruppenFuellen(this.props.tabellen, this.state.runde);
    this.setState({ grplist: gf });
    this.setState({ manID: [] });
    this.setState({ grpID: [] });
  }

  render() {
    const head = (
      <div>
        {'Grunddaten > Runden > '}
        <b>Gruppen</b>
        {' > Spiele'}
      </div>
    );

    const sMID = this.state.manID ? this.state.manID : '';
    const sGID = this.state.grpID ? this.state.grpID : '';

    let message = '';
    let disableWeiter = false;
    const t = this.props.tabellen.filter((x) => x.Runde > 1 && x.Mannschaft === '');

    if (t.length > 0) {
      message = 'Nicht alle Gruppen komplett!';
      disableWeiter = true;
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

          <div className="form-group row mr-1">
            <label htmlFor="runde" className="col-sm-3 col-form-label">Runden-Nr.</label>
            <div className="col-sm-5">
              <input
                type="number"
                className="form-control form-control-sm"
                min="2"
                max={this.props.runden.length}
                id="runde"
                value={this.state.runde}
                onChange={this.rundeChange.bind(this)}
              />
            </div>
          </div>

          <div className="form-group row mr-1">
            <label htmlFor="platz" className="col-sm-3 col-form-label">Mannschaften</label>
            <div className="col-sm-5">
              <DropdownWithID
                id="platz"
                values={
                  [
                    { ID: '1', Text: 'nur Mannschaften der Vorrunde' },
                    { ID: '2', Text: 'Mannschaften aller Vorrunden' },
                    { ID: '3', Text: 'bester n-te der Vorrunde' },
                  ]
                }
                value={this.state.man}
                cName="form-control form-control-sm mt-1 dropdown-toggle text-left"
                wahl={this.MannschaftChange.bind(this)}
              />
            </div>
          </div>
        </div>

        <div className="d-flex flex-row m-2">

          <Table
            id="Man"
            daten={this.state.quellPlaetzeList}
            cols={[{ Column: 'Text', Label: 'Mannschaft' }]}
            chk="Selected"
            chktype="checkbox"
            chkid={sMID}
            radiogrp="Mannschaft"
            height="20vh"
            rowClick={this.rowClickMan.bind(this)}
          />

          <div className="d-flex flex-column mt-4">
            <button
              type="button"
              className="btn btn-primary border-dark m-1"
              onClick={this.Rein.bind(this)}
              title="Mannschaft in Gruppe einfügen"
            >
              {/* <img
                src={require('../Icons/icons8-insert-row-16-withe.png')}
                alt="Rein"
              /> */}
              &gt;&gt;
            </button>
            <button
              type="button"
              className="btn btn-danger border-dark m-1"
              onClick={this.Raus.bind(this)}
              title="Mannschaft aus Gruppe entfernen"
            >
              {/* <img
                src={require('../Icons/icons8-delete-row-16-withe.png')}
                alt="Rein"
              /> */}
              &lt;&lt;
            </button>
          </div>

          <Table
            id="GRP"
            daten={this.state.grplist}
            cols={[{ Column: 'Gruppe', Label: 'Gruppe' }, { Column: 'Mannschaft', Label: 'Mannschaft' }]}
            chk="Selected"
            chktype="checkbox"
            chkid={sGID}
            radiogrp="Gruppe"
            height="20vh"
            rowClick={this.rowClickGrp.bind(this)}
          />

        </div>

        <div className="d-flex flex-row justify-content-between">
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.props.clickWeiter.bind(this, -1)}>
            Zurück
          </button>
          <button type="button" className="btn btn-primary border-dark m-1" onClick={this.props.clickWeiter.bind(this, 1)} disabled={disableWeiter}>
            Weiter
          </button>
        </div>
        <div className="mr-1 pt-1">{message}</div>
      </div>
    );
  }
}

System3.propTypes = {
  SystemKopf: PropTypes.objectOf(TKopf),
  runden: PropTypes.arrayOf(PropTypes.object),
  tabellen: PropTypes.arrayOf(PropTypes.object),
  clickWeiter: PropTypes.func,
  setDaten: PropTypes.func,
  panel: PropTypes.string,
};

System3.defaultProps = {
  SystemKopf: '',
  runden: [],
  tabellen: [],
  clickWeiter: () => {},
  setDaten: () => {},
  panel: 0,
};

export default System3;
