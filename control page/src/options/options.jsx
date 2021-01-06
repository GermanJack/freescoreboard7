import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Display from './display/Display';
import Basics from './basic/Basics';
import Pictures from './pictures/Pictures';
import Events from './events/Events2';
import FontMgr from './fontmgr/FontMgr';
import Penalties from './penalties/Penalties';
import PictureMgr from './picturemgr/PictureMgr';
import SoundMgr from './soundmgr/SoundMgr';
import Sounds from './sounds/Sounds';
import Timer from './timer/Timer';
import Turnier from './turnier/Turnier';

class Options extends Component {
  constructor() {
    super();
    this.state = {
      linkid: 10,
    };
  }

  setLink(linkid) {
    this.setState({ linkid });
  }

  render() {
    let p = null;
    if (this.state.linkid === 10) {
      p = (
        <Display
          options={this.props.options}
          anzeige={this.props.anzeige}
          PageSets={this.props.PageSets}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 20) {
      p = (
        <Timer
          timer={this.props.timer}
          timerevents={this.props.timerevents}
          soundList={this.props.soundList}
          pages={this.props.seiten}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 30) {
      p = (
        <Pictures
          picList={this.props.picList}
          options={this.props.options}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 50) {
      p = (
        <Sounds
          soundList={this.props.soundList}
          options={this.props.options}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 60) {
      p = (
        <Basics
          options={this.props.options}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 70) {
      p = (
        <Penalties
          penalties={this.props.penalties}
          options={this.props.options}
          soundList={this.props.soundList}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 75) {
      p = (
        <Turnier
          options={this.props.options}
          tabsort={this.props.tabsort}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 80) {
      p = (
        <Events
          options={this.props.options}
          events={this.props.events}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    if (this.state.linkid === 90) {
      p = (
        <PictureMgr
          options={this.props.options}
          picList={this.props.picList}
          websocketSend={this.props.websocketSend.bind(this)}
          websocketSendRaw={this.props.websocketSendRaw.bind(this)}
        />
      );
    }

    if (this.state.linkid === 100) {
      p = (
        <SoundMgr
          soundList={this.props.soundList}
          websocketSend={this.props.websocketSend.bind(this)}
          websocketSendRaw={this.props.websocketSendRaw.bind(this)}
        />
      );
    }

    if (this.state.linkid === 110) {
      p = (
        <FontMgr
          fontList={this.props.fontList}
          websocketSend={this.props.websocketSend.bind(this)}
          websocketSendRaw={this.props.websocketSendRaw.bind(this)}
        />
      );
    }

    const btnon = 'm-1 p-1 btn btn-dark';
    const btnoff = 'm-1 p-1 btn btn-outline-dark';

    return (
      <div className="">

        <div className="text-center bg-fsc text-light p-1 mb-1">
          Einstellungen
        </div>

        <div className="d-flex flex-row overflow-hidden border border-primary">

          <div className="border border-secondary">
            <nav className="nav flex-column">
              <button type="button" className={this.state.linkid === 10 ? btnon : btnoff} onClick={this.setLink.bind(this, 10)}>Anzeige</button>
              <button type="button" className={this.state.linkid === 20 ? btnon : btnoff} onClick={this.setLink.bind(this, 20)}>Uhren</button>
              <button type="button" className={this.state.linkid === 30 ? btnon : btnoff} onClick={this.setLink.bind(this, 30)}>Bilder</button>
              <button type="button" className={this.state.linkid === 50 ? btnon : btnoff} onClick={this.setLink.bind(this, 50)}>Töne</button>
              <button type="button" className={this.state.linkid === 60 ? btnon : btnoff} onClick={this.setLink.bind(this, 60)}>Audiowiedergabe</button>
              <button type="button" className={this.state.linkid === 70 ? btnon : btnoff} onClick={this.setLink.bind(this, 70)}>Strafen</button>
              <button type="button" className={this.state.linkid === 75 ? btnon : btnoff} onClick={this.setLink.bind(this, 75)}>Turnier</button>
              <button type="button" className={this.state.linkid === 80 ? btnon : btnoff} onClick={this.setLink.bind(this, 80)}>Ereignisprotokoll</button>
              <button type="button" className={this.state.linkid === 90 ? btnon : btnoff} onClick={this.setLink.bind(this, 90)}>Bilderverwaltung</button>
              <button type="button" className={this.state.linkid === 100 ? btnon : btnoff} onClick={this.setLink.bind(this, 100)}>Töneverwaltung</button>
              <button type="button" className={this.state.linkid === 110 ? btnon : btnoff} onClick={this.setLink.bind(this, 110)}>Schriftverwaltung</button>
            </nav>
          </div>

          <div>
            {p}
          </div>

        </div>
      </div>
    );
  }
}

Options.propTypes = {
  options: PropTypes.oneOfType(PropTypes.object),
  tabsort: PropTypes.oneOfType(PropTypes.object),
  soundList: PropTypes.oneOfType(PropTypes.object),
  fontList: PropTypes.OfType(PropTypes.object),
  anzeige: PropTypes.oneOfType(PropTypes.object),
  picList: PropTypes.OfType(PropTypes.object),
  PageSets: PropTypes.OfType(PropTypes.object),
  timer: PropTypes.OfType(PropTypes.object),
  timerevents: PropTypes.OfType(PropTypes.object),
  penalties: PropTypes.OfType(PropTypes.object),
  events: PropTypes.OfType(PropTypes.object),
  seiten: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func,
  websocketSendRaw: PropTypes.func,
};

Options.defaultProps = {
  options: [],
  tabsort: [],
  soundList: [],
  fontList: [],
  picList: [],
  anzeige: null,
  PageSets: [],
  timer: [],
  timerevents: [],
  penalties: [],
  events: [],
  websocketSend: () => {},
  websocketSendRaw: () => {},
};

export default Options;
