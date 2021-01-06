import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Fenster from './Fenster';

class Kontrolle3S extends Component {
  constructor() {
    super();
    this.state = {
      DragObjektID: 0,
      DropBoxcolum: 0,
    };
  }

  // open minimize window
  handleToggleFenster(id) {
    const fenster = this.props.WebKontrols;
    const index = fenster.findIndex((x) => x.ID === id);
    if (fenster[index].Offen === 1) {
      fenster[index].Offen = 0;
    } else {
      fenster[index].Offen = 1;
    }
    const fjson = JSON.stringify(fenster[index]);
    this.props.websocketSend({ Type: 'bef', Command: 'SaveWebKontrol', Value1: fjson });
  }

  // close window
  handleClsFenster(id) {
    const fenster = this.props.WebKontrols;
    const index = fenster.findIndex((x) => x.ID === id);
    fenster[index].Sichtbar = 0;
    const fjson = JSON.stringify(fenster[index]);
    this.props.websocketSend({ Type: 'bef', Command: 'SaveWebKontrol', Value1: fjson });
  }

  handleAddFenster(Fenster1) {
    const Fensters = this.props.WebKontrols;
    Fensters.push(Fenster1);
    // this.setState({ Fenster: Fensters });
    // this.props.onFensterChange(Fensters);
  }

  handleDrag(id) {
    this.setState({ DragObjektID: id });
  }

  // drop inside a column
  handleDrop(id) {
    // Drop to other window
    const fensterArray = this.props.WebKontrols;
    const dragedIndex = fensterArray.findIndex((x) => x.ID === this.state.DragObjektID);
    const dropedIndex = fensterArray.findIndex((x) => x.ID === id);
    fensterArray[dragedIndex].Spalte = fensterArray[dropedIndex].Spalte;
    fensterArray[dragedIndex].Sort = fensterArray[dropedIndex].Sort - 1;

    const fjson = JSON.stringify(fensterArray[dragedIndex]);
    this.props.websocketSend({ Type: 'bef', Command: 'SaveWebKontrol', Value1: fjson });

    this.setState({ DropBoxcolum: 0 });
  }

  sortFenstercolumn(fensterArray) {
    // renumber all windows
    const newarr = fensterArray;
    for (let s = 1; s <= 3; s += 1) {
      const fensterArray2 = this.columnFenster(fensterArray, s);
      for (let i = 0; i < fensterArray2.length; i += 1) {
        const index = this.findIndex(fensterArray, fensterArray2[i].id);
        newarr[index].sort = i * 10;
      }
    }
    return newarr;
  }

  columnFenster(fensterArray, colum) {
    return fensterArray.filter((f) => f.colum === colum).sort((a, b) => a.Sort - b.Sort);
  }

  findIndex(fensterArray, id) {
    return fensterArray.findIndex((x) => x.ID === id);
  }

  handleOnDrop(target) {
    const fenster = this.props.WebKontrols;
    const index = fenster.findIndex((x) => x.ID === this.state.DragObjektID);
    fenster[index].Spalte = target;
    // this.setState(Fenster[index] = fenster[index]);

    const fjson = JSON.stringify(fenster[index]);
    this.props.websocketSend({ Type: 'bef', Command: 'SaveWebKontrol', Value1: fjson });
  }

  handleDragOvercolum(colum) {
    this.setState({ DropBoxcolum: colum });
  }

  handleDragOver(e) {
    e.preventDefault();
  }

  // move a windows between columns to the drop area at the bottom
  handleDropcolum() {
    // Drop to column
    let fensterArray = this.props.WebKontrols;
    const dragedIndex = fensterArray.findIndex((x) => x.ID === this.state.DragObjektID);
    fensterArray[dragedIndex].Spalte = this.state.DropBoxcolum;

    // find highes Sort of drop column
    const colfen = fensterArray.filter((f) => f.Spalte === this.state.DropBoxcolum);
    const maxsort = Math.max(...colfen.map((o) => o.Sort));

    fensterArray[dragedIndex].Sort = maxsort + 1;

    fensterArray = this.sortFenstercolumn(fensterArray);

    const fjson = JSON.stringify(fensterArray[dragedIndex]);
    this.props.websocketSend({ Type: 'bef', Command: 'SaveWebKontrol', Value1: fjson });

    this.setState({ DropBoxcolum: 0 });
  }

  ResetFenster(id) {
    const x = this.findIndex(this.props.WebKontrols, id);
    x.window.reset();
  }

  handleBefehl(Befehl, Wert) {
    this.props.websocketSend.send({
      Domain: 'KO', Type: 'bef', Command: Befehl, Value1: Wert,
    });
  }

  render() {
    const Fvisible = this.props.WebKontrols.filter((fenster) => fenster.Sichtbar === 1);

    const F1 = Fvisible
      .filter((fenster) => fenster.Spalte === 1)
      .sort((a, b) => a.Sort - b.Sort)
      .map((window) => (
        <Fenster
          handleToggleFenster={this.handleToggleFenster.bind(this)}
          handleClsFenster={this.handleClsFenster.bind(this)}
          handleReset={this.ResetFenster.bind(this)}
          key={window.id}
          window={window}
          handleDrag={this.handleDrag.bind(this)}
          handleDrop={this.handleDrop.bind(this)}
          handleBefehl={this.handleBefehl.bind(this)}
          websocketSend={this.props.websocketSend.bind(this)}
          textvariablen={this.props.textvariablen}
          picVariables={this.props.picVariables}
          tabellenvariablen={this.props.tabellenvariablen}
          teamList={this.props.teamList}
          playerlistTeam1={this.props.playerlistTeam1}
          playerlistTeam2={this.props.playerlistTeam2}
          options={this.props.options}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          penalties={this.props.penalties}
          anzeigetabellen={this.props.anzeigetabellen}
          turnierID={this.props.turnierID}
          turnier={this.props.turnier}
          pages={this.props.pages}
        />
      ));

    const F2 = Fvisible
      .filter((fenster) => fenster.Spalte === 2)
      .sort((a, b) => a.Sort - b.Sort)
      .map((window) => (
        <Fenster
          handleToggleFenster={this.handleToggleFenster.bind(this)}
          handleClsFenster={this.handleClsFenster.bind(this)}
          handleReset={this.ResetFenster.bind(this)}
          key={window.id}
          window={window}
          handleDrag={this.handleDrag.bind(this)}
          handleDrop={this.handleDrop.bind(this)}
          handleBefehl={this.handleBefehl.bind(this)}
          websocketSend={this.props.websocketSend}
          textvariablen={this.props.textvariablen}
          picVariables={this.props.picVariables}
          tabellenvariablen={this.props.tabellenvariablen}
          teamList={this.props.teamList}
          playerlistTeam1={this.props.playerlistTeam1}
          playerlistTeam2={this.props.playerlistTeam2}
          options={this.props.options}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          penalties={this.props.penalties}
          anzeigetabellen={this.props.anzeigetabellen}
          turnierID={this.props.turnierID}
          turnier={this.props.turnier}
          pages={this.props.pages}
        />
      ));

    const F3 = Fvisible
      .filter((fenster) => fenster.Spalte === 3)
      .sort((a, b) => a.Sort - b.Sort)
      .map((window) => (
        <Fenster
          handleToggleFenster={this.handleToggleFenster.bind(this)}
          handleClsFenster={this.handleClsFenster.bind(this)}
          handleReset={this.ResetFenster.bind(this)}
          key={window.id}
          window={window}
          handleDrag={this.handleDrag.bind(this)}
          handleDrop={this.handleDrop.bind(this)}
          handleBefehl={this.handleBefehl.bind(this)}
          websocketSend={this.props.websocketSend}
          textvariablen={this.props.textvariablen}
          picVariables={this.props.picVariables}
          tabellenvariablen={this.props.tabellenvariablen}
          teamList={this.props.teamList}
          playerlistTeam1={this.props.playerlistTeam1}
          playerlistTeam2={this.props.playerlistTeam2}
          options={this.props.options}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          penalties={this.props.penalties}
          anzeigetabellen={this.props.anzeigetabellen}
          turnierID={this.props.turnierID}
          turnier={this.props.turnier}
          pages={this.props.pages}
        />
      ));

    // drop area at bottom of the columns
    let dropBox = null;
    dropBox = (
      <div
        className="d-flex justify-content-center border border-dark display-3 text-info ml-1 mt-1 rounded"
        onDrop={this.handleDropcolum.bind(this)}
        onDragOver={this.handleDragOver.bind(this)}
      >
        +
      </div>
    );

    return (
      <div className="">
        <div className="row p-0">
          <div className="col p-0" onDragOver={this.handleDragOvercolum.bind(this, 1)}>
            {F1}
            {this.state.DropBoxcolum === 1 ? dropBox : null}
          </div>
          <div className="col p-0" onDragOver={this.handleDragOvercolum.bind(this, 2)}>
            {F2}
            {this.state.DropBoxcolum === 2 ? dropBox : null}
          </div>
          <div className="col p-0" onDragOver={this.handleDragOvercolum.bind(this, 3)}>
            {F3}
            {this.state.DropBoxcolum === 3 ? dropBox : null}
          </div>
        </div>
      </div>
    );
  }
}

Kontrolle3S.propTypes = {
  WebKontrols: PropTypes.arrayOf(PropTypes.object).isRequired,
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  tabellenvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  penalties: PropTypes.arrayOf(PropTypes.object).isRequired,
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  timer: PropTypes.arrayOf(PropTypes.object).isRequired,
  timerObjects: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
  anzeigetabellen: PropTypes.arrayOf(PropTypes.object).isRequired,
  turnierID: PropTypes.string.isRequired,
  pages: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default Kontrolle3S;
