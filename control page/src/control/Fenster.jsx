/* eslint-disable react/no-string-refs */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { IoIosAdd, IoIosRemove, IoMdClose } from 'react-icons/io';
import Tore from './functions/Tore';
import Mannschaften from './functions/Mannschaften';
import Foul from './functions/Foul';
import Zeiten from './functions/Zeiten';
import ZeitenSpielzeit from './functions/ZeitenSpielzeit';
import ZeitenNachspielzeit from './functions/ZeitenNachspielzeit';
import ZeitenPausenzeit from './functions/ZeitenPausenzeit';
import ZeitenfreieZeit from './functions/ZeitenfreieZeit';
import ZeitenSekundenzaehler from './functions/ZeitenSekundenzaehler';
import Turniersteuerung from './functions/Turniersteuerung';
import Laufschrift from './functions/Laufschrift';
import Spielabschnitt from './functions/Spielabschnitt';
import Spieler from './functions/Spieler';
import Paintball from './functions/Paintball';
import Strafen from './functions/Strafen';
import Vorschau from './functions/Vorschau';
import AFootball from './functions/AFootball';
import Spielrichtung from './functions/Spielrichtung';
import BildwahlWin from './functions/BildwahlWin';
import TonwahlWin from './functions/TonwahlWin';
import Saetze from './functions/Saetze';
import Seitenwahl2 from './functions/Seitenwahl2';

import DlgWarning from '../StdComponents/DlgWarning';

class Fenster extends Component {
  constructor(props) {
    super(props);

    // This binding is necessary to make `this` work in the callback
    this.handleMouseDown = this.handleMouseDown.bind(this);
    this.handleBtnCollapse = this.handleBtnCollapse.bind(this);
    this.handleBtnClose =  this.handleBtnClose.bind(this);
  }

  handleMouseDown() {
    // console.log(e);
  }

  handleBtnCollapse(id) {
    this.props.handleToggleFenster(id);
  }

  handleBtnClose(id) {
    this.props.handleClsFenster(id);
  }

  handleBtnReset(id) {
    this.refs[id].reset();
  }

  handleDrag(id) {
    // console.log('Fenster drag '+id);
    this.props.handleDrag(id);
  }

  handleOnDragover(e) {
    e.preventDefault();
  }

  handleDrop(id) {
    this.props.handleDrop(id);
  }

  handleBefehl(e) {
    this.props.handleBefehl(e);
  }

  render() {
    const offen = this.props.window.Offen === 1 ? 'show' : null;
    const contentdivclass = `collapse ${offen} p-1`;
    const zeichen = this.props.window.Offen ? <IoIosRemove /> : <IoIosAdd />;
    const clsZeischen = <IoMdClose />;

    const reset = (
      <button
        type="button"
        className="btn bg-own2 rounded p-0 pl-2 pr-2"
        onClick={this.handleBtnReset.bind(this, this.props.window.Objekt)}
        tabIndex={0}
      >
        reset
      </button>
    );

    const resetWithWarning = (
      <DlgWarning
        label1="Werte wirklich zurücksetzten?"
        btnclassname="btn bg-own2 rounded p-0 pl-2 pr-2"
        iconAltText="r"
        toolTip="reset"
        modalID={`Modal-reset${this.props.window.Objekt}`}
        name={this.handleBtnReset.bind(this, this.props.window.Objekt)}
        btntext="reset"
      />
    );

    const collapseico = (
      <div
        className="bg-own2 rounded pl-2 pr-2"
        onClick={this.handleBtnCollapse.bind(this, this.props.window.ID)}
        role="button"
        tabIndex={0}
      >
        {zeichen}
      </div>
    );

    const closeico = (
      <div
        className="bg-own2 rounded pl-2 pr-2"
        onClick={this.handleBtnClose.bind(this, this.props.window.ID)}
        role="button"
        tabIndex={0}
      >
        {clsZeischen}
      </div>
    );

    let content = '';

    if (this.props.window.Objekt === 'Seitenwahl') {
      content = (
        <Seitenwahl2
          pages={this.props.pages}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Seitenwahl"
        />
      );
    }

    if (this.props.window.Objekt === 'Turniersteuerung') {
      content = (
        <Turniersteuerung
          textvariablen={this.props.textvariablen}
          turnierID={this.props.turnierID}
          turnier={this.props.turnier}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Turniersteuerung"
        />
      );
    }

    if (this.props.window.Objekt === 'Strafen') {
      content = (
        <Strafen
          textvariablen={this.props.textvariablen}
          tabellenvariablen={this.props.tabellenvariablen}
          playerlistTeam1={this.props.playerlistTeam1}
          playerlistTeam2={this.props.playerlistTeam2}
          penalties={this.props.penalties}
          anzeigetabellen={this.props.anzeigetabellen}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Strafen"
        />
      );
    }

    if (this.props.window.Objekt === 'AFootball') {
      content = (
        <AFootball
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="AFootball"
        />
      );
    }

    if (this.props.window.Objekt === 'Vorschau') {
      content = (
        <Vorschau
          ref="Vorschau"
        />
      );
    }

    if (this.props.window.Objekt === 'Mannschaften') {
      content = (
        <Mannschaften
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          teamList={this.props.teamList}
          turnierID={this.props.turnierID}
          ref="Mannschaften"
        />
      );
    }

    if (this.props.window.Objekt === 'Foul') {
      content = (
        <Foul
          options={this.props.options}
          textvariablen={this.props.textvariablen}
          playerlistTeam1={this.props.playerlistTeam1}
          playerlistTeam2={this.props.playerlistTeam2}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Foul"
        />
      );
    }

    if (this.props.window.Objekt === 'Zeiten') {
      content = (
        <Zeiten
          textvariablen={this.props.textvariablen}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Zeiten"
        />
      );
    }

    if (this.props.window.Objekt === 'Spielzeit') {
      content = (
        <ZeitenSpielzeit
          textvariablen={this.props.textvariablen}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Spielzeit"
        />
      );
    }

    if (this.props.window.Objekt === 'Nachspielzeit') {
      content = (
        <ZeitenNachspielzeit
          textvariablen={this.props.textvariablen}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Nachspielzeit"
        />
      );
    }

    if (this.props.window.Objekt === 'Pausenzeit') {
      content = (
        <ZeitenPausenzeit
          textvariablen={this.props.textvariablen}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Pausenzeit"
        />
      );
    }

    if (this.props.window.Objekt === 'freieZeit') {
      content = (
        <ZeitenfreieZeit
          textvariablen={this.props.textvariablen}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="freieZeit"
        />
      );
    }

    if (this.props.window.Objekt === 'Sekundenzähler') {
      content = (
        <ZeitenSekundenzaehler
          textvariablen={this.props.textvariablen}
          timer={this.props.timer}
          timerObjects={this.props.timerObjects}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Sekundenzähler"
        />
      );
    }

    if (this.props.window.Objekt === 'Laufschrift') {
      content = (
        <Laufschrift
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Laufschrift"
        />
      );
    }

    if (this.props.window.Objekt === 'Spielabschnitt') {
      content = (
        <Spielabschnitt
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Spielabschnitt"
        />
      );
    }

    if (this.props.window.Objekt === 'Spielrichtung') {
      content = (
        <Spielrichtung
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Spielrichtung"
        />
      );
    }

    if (this.props.window.Objekt === 'Spieler') {
      content = (
        <Spieler
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          playerlistTeam1={this.props.playerlistTeam1}
          playerlistTeam2={this.props.playerlistTeam2}
          teamList={this.props.teamList}
          ref="Spieler"
        />
      );
    }

    if (this.props.window.Objekt === 'Paintball') {
      content = (
        <Paintball
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Paintball"
        />
      );
    }

    if (this.props.window.Objekt === 'Tore') {
      content = (
        <Tore
          textvariablen={this.props.textvariablen}
          options={this.props.options}
          playerlistTeam1={this.props.playerlistTeam1}
          playerlistTeam2={this.props.playerlistTeam2}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Tore"
        />
      );
    }

    if (this.props.window.Objekt === 'Saetze') {
      content = (
        <Saetze
          textvariablen={this.props.textvariablen}
          websocketSend={this.props.websocketSend.bind(this)}
          ref="Saetze"
        />
      );
    }

    if (this.props.window.Objekt === 'BildwahlWin') {
      content = (
        <BildwahlWin
          websocketSend={this.props.websocketSend.bind(this)}
          options={this.props.options}
          ref="BildwahlWin"
        />
      );
    }

    if (this.props.window.Objekt === 'TonwahlWin') {
      content = (
        <TonwahlWin
          websocketSend={this.props.websocketSend.bind(this)}
          options={this.props.options}
          ref="TonwahlWin"
        />
      );
    }

    return (
      // Fensterrahmen
      <div
        className="d-flex flex-column mt-1 ml-1 border border-dark rounded"
        draggable="true"
        onDrag={this.handleDrag.bind(this, this.props.window.ID)}
        onDragOver={this.handleOnDragover.bind(this)}
        onDrop={this.handleDrop.bind(this, this.props.window.ID)}
      >

        {/* Titelleiste */}
        <div className="d-flex justify-content-between bg-own">
          {/* Titel */}
          <div id="KSpielsteuerungTitel" className="text-white pl-1">
            {this.props.window.Title}
          </div>
          {/* reset und colapse btn */}
          <div className="d-flex justify-content-between">
            {this.props.window.Reset === 1 ? reset : null}
            {this.props.window.Reset === 2 ? resetWithWarning : null}
            {collapseico}
            {closeico}
          </div>
        </div>

        {/* Inhalt */}
        <div className={contentdivclass}>
          {content}
        </div>
      </div>
    );
  }
}

Fenster.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  // picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  tabellenvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  anzeigetabellen: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  penalties: PropTypes.arrayOf(PropTypes.object).isRequired,
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  timer: PropTypes.arrayOf(PropTypes.object).isRequired,
  timerObjects: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
  handleToggleFenster: PropTypes.func.isRequired,
  handleClsFenster: PropTypes.func.isRequired,
  window: PropTypes.oneOfType(PropTypes.object).isRequired,
  handleDrag: PropTypes.func.isRequired,
  handleDrop: PropTypes.func.isRequired,
  handleBefehl: PropTypes.func.isRequired,
  turnierID: PropTypes.string.isRequired,
  turnier: PropTypes.string.isRequired,
  pages: PropTypes.arrayOf(PropTypes.string).isRequired,
};

export default Fenster;
