import React, { Component } from 'react';
import PropTypes from 'prop-types';
// import Table from '../StdComponents/Table';
import TreeView from 'deni-react-treeview';
import Dropdown from '../StdComponents/Dropdown';

import './tree-theme.scss';
import Turniermenue from './Turniermenue';
import Systemmenue from './Systemmenue';
import GraphCalc from './GraphCalc';
// import { Graphviz } from 'graphviz-react';

class Turniere extends Component {
  constructor() {
    super();
    this.state = {
      turnierID: 0,
      turnierText: [],
      path: '',
      turnierTyp: '',
      tableData: [],
    };

    this.treeRef = React.createRef();
  }

  componentDidMount() {
    this.typeSelection('Turniere');
  }

  componentDidUpdate(prevProps) {
    if (prevProps.turniere !== this.props.turniere) {
      // eslint-disable-next-line react/no-did-update-set-state
      this.setState({ tableData: this.jsonstring() });
    }
  }

  onSelectItem(item) {
    this.setPath(item);
    this.setTurnierText(item);
    this.setState({ turnierID: item.turnierKopf.ID });
  }

  onExpanded(item) {
    this.setPath(item);
    this.setTurnierText(item);
    this.setState({ turnierID: item.turnierKopf.ID });
  }

  onColapsed(item) {
    this.setPath(item);
    this.setTurnierText(item);
    this.setState({ turnierID: item.turnierKopf.ID });
  }

  setPath(item) {
    if (item === null) {
      this.setState({ path: '' });
      return;
    }

    switch (item.level) {
      case 0:
        this.setState({ path: `T: ${item.text}` });
        break;
      case 1:
        this.setState({ path: `T: ${item.tnr} - ${item.text}` });
        break;
      case 2:
        this.setState({ path: `T: ${item.tnr} - R: ${item.rnr} - ${item.text}` });
        break;
      case 3:
        this.setState({ path: `T: ${item.tnr} - R: ${item.rnr} - G: ${item.gnr} - S: ${item.text}` });
        break;
      default:
        this.setState({ path: '' });
    }
  }

  setTurnierText(item) {
    if (item === null) {
      this.setState({ turnierText: [] });
      return;
    }

    const text = [];
    text.push(`${item.turnierKopf.Turniertyp}Nr.: ${item.turnierKopf.TurnierNr} (${item.turnierKopf.ID})`);
    text.push(`Name: ${item.turnierKopf.Beschreibung}`);
    text.push(`Mannschaften: ${item.turnierKopf.Mananz}`);
    if (this.state.turnierTyp === 'Turniere') {
      text.push(`Liga: ${item.turnierKopf.Liga}`);
      text.push(`Turniersystem: ${item.turnierKopf.Matrix}`);
    }
    text.push('--------------------');
    text.push(`${item.turnierKopf.Kommentar}`);
    this.setState({ turnierText: text });
  }


  getMatchs(turnier, groupID) {
    if (!groupID) {
      return [{ id: 0, text: 'keine Gruppe' }];
    }

    const matchs = [];
    const g = turnier.Spiele.filter((x) => x.Gruppe === groupID);
    for (let i = 0; i < g.length; i += 1) {
      matchs.push(
        {
          id: g[i].ID,
          level: 3,
          turnierKopf: turnier.Kopf,
          tnr: turnier.Kopf.Beschreibung,
          rnr: groupID.substring(0, groupID.length - 1),
          gnr: groupID,
          sdata: g[i],
          text: `${g[i].IstMannA} : ${g[i].IstMannB} (${g[i].ToreA}:${g[i].ToreB})`,
          children: [],
        },
      );
    }

    return matchs;
  }

  getGroups(turnier, roundID) {
    if (!roundID) {
      return [{ id: 0, text: 'keine Runde' }];
    }

    const groups = [];
    const g = turnier.Gruppen.filter((x) => x.Runde === roundID);
    for (let i = 0; i < g.length; i += 1) {
      groups.push(
        {
          id: g[i].ID,
          level: 2,
          turnierKopf: turnier.Kopf,
          tnr: turnier.Kopf.Beschreibung,
          rnr: roundID,
          gnr: g[i].Gruppe,
          text: `Gruppe: ${g[i].Gruppe}`,
          children: this.getMatchs(turnier, g[i].Gruppe),
        },
      );
    }

    return groups;
  }

  getRounds(turnier) {
    if (!turnier) {
      return [{ id: 0, text: 'kein Turnier' }];
    }

    const runden = [];
    for (let i = 0; i < turnier.Runden.length; i += 1) {
      runden.push(
        {
          id: turnier.Runden[i].ID,
          level: 1,
          turnierKopf: turnier.Kopf,
          tnr: turnier.Kopf.Beschreibung,
          rnr: turnier.Runden[i].Runde,
          text: `Runde: ${turnier.Runden[i].Runde}`,
          children: this.getGroups(turnier, turnier.Runden[i].Runde),
        },
      );
    }

    return runden;
  }

  jsonstring() {
    const tdata = [];

    const filter = this.state.turnierTyp;

    for (let i = 0; i < this.props.turniere.length; i += 1) {
      if (this.props.turniere[i].Kopf.Turniertyp === filter) {
        tdata.push(
          {
            id: this.props.turniere[i].ID,
            level: 0,
            turnierKopf: this.props.turniere[i].Kopf,
            tnr: this.props.turniere[i].Kopf.Beschreibung,
            text: this.props.turniere[i].Kopf.Beschreibung,
            children: this.getRounds(this.props.turniere[i]),
          },
        );
      }
    }

    return tdata;
  }

  namensliste() {
    const data = [];

    const filter = this.state.turnierTyp;

    for (let i = 0; i < this.props.turniere.length; i += 1) {
      if (this.props.turniere[i].Kopf.Turniertyp === filter) {
        data.push(this.props.turniere[i].Kopf.Beschreibung);
      }
    }

    return data;
  }

  systeme() {
    const data = [];
    for (let i = 0; i < this.props.turniere.length; i += 1) {
      if (this.props.turniere[i].Kopf.Turniertyp === 'System') {
        data.push(this.props.turniere[i]);
      }
    }
    return data;
  }

  typeSelection(e) {
    this.setPath(null);
    this.setTurnierText(null);
    this.setState({ turnierID: 0 });

    if (e === 'Turniere') {
      this.setState({ turnierTyp: 'Turnier' });
      this.props.websocketSend({ Type: 'req', Command: 'TurniereKomplett' });
    } else {
      this.setState({ turnierTyp: 'System' });
      this.props.websocketSend({ Type: 'req', Command: 'TurniereKomplett' });
    }
  }

  turnierDelete(e) {
    let t = 'DelSystem';
    if (e !== 'System') {
      t = 'DelTurnier';
    }

    this.props.websocketSend({ Type: 'bef', Command: t, Value1: this.state.turnierID });
  }

  grafik() {
    const t = this.props.turniere.filter((x) => x.Kopf.ID === this.state.turnierID);
    const g = GraphCalc(t[0]);
    // eslint-disable-next-line no-console
    console.log(g);
    // return (
    //   <Graphviz
    //     dot={g}
    //     fit="true"
    //     height="500"
    //     width="500"
    //     zoom="false"
    //   />
    // );
  }

  render() {
    // eslint-disable-next-line arrow-body-style
    const txt = this.state.turnierText.map((x) => {
      return (
        <span key={x} style={{ whiteSpace: 'pre-wrap' }}>
          {x.replace('\r\n', '<br>')}
          <br />
        </span>
      );
    });

    let menue = (
      <Turniermenue
        namensliste={this.namensliste()}
        websocketSend={this.props.websocketSend.bind(this)}
        turnierDelete={this.turnierDelete.bind(this)}
        grafik={this.grafik.bind(this)}
        systeme={this.systeme()}
        teamList={this.props.teamList}
        turnierID={this.state.turnierID}
      />
    );
    if (this.state.turnierTyp !== 'Turnier') {
      menue = (
        <Systemmenue
          namensliste={this.namensliste()}
          websocketSend={this.props.websocketSend.bind(this)}
          turnierDelete={this.turnierDelete.bind(this)}
          grafik={this.grafik.bind(this)}
        />
      );
    }

    return (
      <div className="d-flex flex-column">

        <div className="text-center bg-fsc text-light p-1">
          Verwaltung Turniere / Turniersysteme
        </div>

        <div className="d-flex flex-row p-1 justify-content-start">
          <div className="pr-2">
            Turniere oder Turniersysteme:
          </div>
          <Dropdown
            values={['Turniere', 'Systeme']}
            value={this.state.turnierTyp}
            toolTip="Turniere oder Turniersysteme"
            wahl={this.typeSelection.bind(this)}
          />
        </div>
        <div className="d-flex flex-row">

          <div className="theme-customization d-flex flex-column w-30">
            <TreeView
              className="treeview-teste"
              marginItems={15}
              showIcon={false}
              items={this.state.tableData}
              onSelectItem={this.onSelectItem.bind(this)}
              onExpanded={this.onSelectItem.bind(this)}
              onColapsed={this.onSelectItem.bind(this)}
            />
          </div>

          <div className="d-flex flex-column ml-1">
            {menue}
            <div className="p-1 border border-dark">
              {this.state.path}
            </div>
            <div className="p-1 border border-dark">
              {txt}
            </div>
          </div>

        </div>

      </div>
    );
  }
}

Turniere.propTypes = {
  turniere: PropTypes.arrayOf(PropTypes.arrayOf.objects),
  teamList: PropTypes.arrayOf(PropTypes.object),
  websocketSend: PropTypes.func,
};

Turniere.defaultProps = {
  websocketSend: () => {},
  turniere: [],
  teamList: [],
};

export default Turniere;
