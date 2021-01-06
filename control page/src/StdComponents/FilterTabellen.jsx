import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoIssueReopened } from 'react-icons/go';
import { IoMdTrash } from 'react-icons/io';
import Table from './Table';
import Dropdown from './Dropdown';
import DownloadJSon from './DownloadJSon';
import DlgFeldwahl from './DlgFeldwahl';
import DlgWarning from './DlgWarning';
import DlgEventNeu from '../turniere/Ereignisse/DlgEventNeu';

import ClsTurnier from '../turniere/dataClasses/ClsTurnier';

class FilterTabellen extends Component {
  constructor() {
    super();
    this.state = {
      selpage: 1,
      selIDs: [],
    };
  }

  getFieldValues(tabField) {
    if (this.props.tabellenvariablen.length === 0) {
      return [];
    }
    const variable = this.props.tabellenvariablen.find((x) => x.ID === this.props.tabVariable);
    const data = JSON.parse(variable.Wert);
    if (data === null) {
      return [];
    }
    if (data.length === 0) {
      return [];
    }
    const werte = [];
    for (let i = 0; i < data.length; i += 1) {
      const w = data[i][tabField];
      if (werte.indexOf(w) === -1) {
        werte.push(w);
      }
    }
    return werte;
  }

  wahl(prop, wert) {
    let uebergabewert = wert;
    if (wert === 'Filter aus') {
      uebergabewert = '';
    }

    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'SetTablefilter',
      Value1: this.props.tabVariable,
      Value2: prop,
      Value3: uebergabewert,
    });
  }

  handleRecPerPageChange(e) {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'RecPerPageChange',
      Value1: this.props.tabVariable,
      Value2: e.target.value,
    });
  }

  handlePageSelection(e) {
    this.setState({ selpage: e });
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'TablePageSelection',
      Value1: this.props.tabVariable,
      Value2: e,
    });
  }

  rowClick(ID) {
    this.setState({ selIDs: [ID] });
  }

  delErgRow() {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'DelEvent',
      Value1: this.state.selIDs[0],
    });
  }

  SpielReopen(spiel) {
    const s = JSON.stringify(spiel);
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'ReOpenMatch',
      Value1: s,
    });
    // alert("Spiel geöffnet");
  }

  render() {
    let data = null;
    let variable = '';
    let RecPerPage = 0;
    let AmountOfPages = 1;
    let delErgeignis = null;
    let neuesEreignis = null;
    let downLoadName = 'tabelle.csv';

    // Tabellen
    if (this.props.tabVariable === 'T01') {
      downLoadName = 'Tabellen.csv';
    }

    // Torschützen
    if (this.props.tabVariable === 'T06') {
      downLoadName = 'Torschützen.csv';
    }

    // Ereignisse
    if (this.props.tabVariable === 'T03') {
      downLoadName = 'Ereignisse.csv';
      if (this.state.selIDs.length > 0) {
        delErgeignis = (
          <DlgWarning
            modalID="DelEvent"
            toolTip="Ereignis löschen"
            reacticon={<IoMdTrash size="1.2em" />}
            btnclassname="btn btn-outline-secondary btn-icon btn-sm ml-1"
            label1="Ereignis wirklich löschen?"
            name={this.delErgRow.bind(this)}
          />
        );
      }

      neuesEreignis = (
        <DlgEventNeu
          modalID="neuesEreignis"
          toolTip="neues Ereignis"
          reacticon={<GoPlus size="1.2em" />}
          btnclassname="btn btn-outline-secondary btn-icon btn-sm ml-1"
          label="neues Ereignis"
          namensliste=""
          textvariablen={this.props.textvariablen}
          teamList={this.props.teamList}
          playerListAll={this.props.playerListAll}
          spiele={this.props.spiele}
          events={this.props.events}
          websocketSend={this.props.websocketSend}
          turnier={this.props.turnier}
        />
      );
    }

    let SpielReOpen = null;
    // Spiele
    if (this.props.tabVariable === 'T02') {
      downLoadName = 'Spielplan.csv';
      if (this.state.selIDs.length > 0) {
        const spiel = this.props.turnier[0].Spiele.find((x) => x.ID === this.state.selIDs[0]);
        const runde = this.props.turnier[0].Runden.find((x) => x.Runde === spiel.Runde);
        if ((spiel.Status === 3 || spiel.Status === 4) && runde.status !== 3) {
          SpielReOpen = (
            <DlgWarning
              modalID="spielreopen"
              label1="Spiel wieder öffnen?"
              toolTip="Spiel wieder öffnen"
              reacticon={<GoIssueReopened size="1.2rem" />}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm ml-1"
              name={this.SpielReopen.bind(this, spiel)}
            />
          );
        }
      }
    }

    // Seitenzahlen ermitteln
    const PageNumbers = [];
    if (this.props.tabellenvariablen.length > 0) {
      variable = this.props.tabellenvariablen.find((x) => x.ID === this.props.tabVariable);
      data = JSON.parse(variable.Wert);
      RecPerPage = variable.RecPerPage;
      AmountOfPages = variable.AmountOfPages;
      if (AmountOfPages === 0) {
        AmountOfPages = 1;
      }
      if (AmountOfPages >= 1) {
        for (let i = 1; i <= AmountOfPages; i += 1) {
          PageNumbers.push(i.toString());
        }
      }
    }

    // Filter felder ermitteln
    let filtFelder = [];
    if (this.props.tableFilter !== null) {
      if (this.props.tableFilter.length > 0) {
        filtFelder = this.props.tableFilter.filter(
          (x) => x.Table === this.props.tabVariable,
        ).map((y) => y.Field);
      }
    }
    const index = filtFelder.indexOf('SelPage');
    if (index > -1) {
      filtFelder.splice(index, 1);
    }

    // Dropdownlisten für Filterfelöder erstellen
    const filt = filtFelder.map((x) => {
      let istWert = '';
      if (this.props.tableFilter) {
        const f = this.props.tableFilter.find(
          (y) => y.Table === this.props.tabVariable && y.Field === x,
        );
        if (f) {
          istWert = f.Value;
        }
      }
      let list = [];
      if (istWert !== '') {
        list.unshift('Filter aus');
      } else {
        list = this.getFieldValues(x);
      }

      return (
        <div className="d-flex flex-row">
          <div className="ml-1 mr-1">
            {x}
            :
          </div>
          <Dropdown
            values={list}
            value={istWert}
            wahl={this.wahl.bind(this, x)}
          />
        </div>
      );
    });

    // Feldfilter setzten
    const TabFelder = [];
    for (let i = 0; i < this.props.anzeigetabellen.length; i += 1) {
      const o = this.props.anzeigetabellen[i];
      if (o.Tabelle === this.props.tabVariable && o.ausblendbar === 1) {
        TabFelder.push(o);
      }
    }

    return (
      <div className=" ">
        <div className="d-flex flex-column m-1">
          <div className="d-flex flex-row m-1">
            <div className="">
              <b>Filter:</b>
            </div>
            {filt}
            <div className="ml-1 mr-1">
              Zeilen / Seite:
            </div>
            <input
              type="number"
              className="form-control"
              style={{ width: '100px' }}
              min="1"
              max="100"
              step="5"
              value={RecPerPage}
              onChange={this.handleRecPerPageChange.bind(this)}
            />
            <div className="ml-1 mr-1">
              Seite:
            </div>
            <Dropdown
              values={PageNumbers}
              value={this.state.selpage}
              wahl={this.handlePageSelection.bind(this)}
            />

            <div className="ml-4 mr-1">
              <b>Tools:</b>
            </div>
            <DlgFeldwahl
              werte={TabFelder}
              label1="Feldwahl"
              toolTip="Feldwahl"
              modalID={`FeldwahlEreignisse_${this.props.tabVariable}`}
              // onChange={this.props.handleFenVis.bind(this)}
              websocketSend={this.props.websocketSend.bind(this)}
              class="btn btn-outline-secondary btn-icon btn-sm mr-1"
            />

            <DownloadJSon
              json={data}
              filename={downLoadName}
              toolTip="csv Download"
            />

            {/* {neuesEreignis} */}

            {delErgeignis}

            {SpielReOpen}

          </div>
        </div>

        <Table
          daten={data}
          chk={`id_${this.props.tabVariable}`}
          cols={[]}
          chkid={this.state.selIDs}
          chktype="radio"
          radiogrp="this.props.tabVariable"
          rowClick={this.rowClick.bind(this)}
        />
      </div>
    );
  }
}

FilterTabellen.propTypes = {
  tabVariable: PropTypes.string,
  tableFilter: PropTypes.arrayOf(PropTypes.object),
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  tabellenvariablen: PropTypes.arrayOf(PropTypes.object),
  anzeigetabellen: PropTypes.arrayOf(PropTypes.object),
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerListAll: PropTypes.arrayOf(PropTypes.object).isRequired,
  spiele: PropTypes.arrayOf(PropTypes.object).isRequired,
  events: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
  turnier: PropTypes.instanceOf(ClsTurnier).isRequired,
};

FilterTabellen.defaultProps = {
  tabVariable: 'T00',
  tableFilter: [],
  anzeigetabellen: [],
  tabellenvariablen: '',
};

export default FilterTabellen;
