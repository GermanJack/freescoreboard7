/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Basics extends Component {
  setPropValue(property, e) {
    const value = e.target.checked;
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: property,
      Value2: value,
    });
  }

  render() {
    const ServerAudio = (this.props.options.filter((x) => x.Prop === 'ServerAudio')[0].Value === 'True');
    const KontrolleAudio = (this.props.options.filter((x) => x.Prop === 'KontrolleAudio')[0].Value === 'True');
    const AnzeigeAudio = (this.props.options.filter((x) => x.Prop === 'AnzeigeAudio')[0].Value === 'True');

    return (
      <div className="p-1 ml-2">
        <u>Audiowiedergabe</u>
        <div className="mt-3">
          Audiogerät:
        </div>
        <table className="table table-sm p-0 m-0 bs-0 table-bordered">
          <thead className="thead-light">
            <tr className="pb-1">
              <td>Gerät</td>
              <td className="border-right">ein/aus</td>
            </tr>
          </thead>
          <tbody style={{ overflow: 'auto' }}>
            <tr className="pb-1">
              <td>
                Server
              </td>
              <td className="border-right">
                <label htmlFor="ServerAudio" className="switch pl-2 pr-2 ml-1 text-center">
                  <input
                    type="checkbox"
                    id="ServerAudio"
                    checked={ServerAudio}
                    onChange={this.setPropValue.bind(this, 'ServerAudio')}
                  />
                  <span className="slider round" />
                </label>
              </td>
            </tr>
            <tr className="pb-1">
              <td>
                Kontrollfenster
              </td>
              <td className="border-right">
                <label htmlFor="KontrolleAudio" className="switch pl-2 pr-2 ml-1 text-center">
                  <input
                    type="checkbox"
                    id="KontrolleAudio"
                    checked={KontrolleAudio}
                    onChange={this.setPropValue.bind(this, 'KontrolleAudio')}
                  />
                  <span className="slider round" />
                </label>
              </td>
            </tr>
            <tr className="pb-1">
              <td>
                Anzeigefenster
              </td>
              <td className="border-right">
                <label htmlFor="AnzeigeAudio" className="switch pl-2 pr-2 ml-1 text-center">
                  <input
                    type="checkbox"
                    id="AnzeigeAudio"
                    checked={AnzeigeAudio}
                    onChange={this.setPropValue.bind(this, 'AnzeigeAudio')}
                  />
                  <span className="slider round" />
                </label>
              </td>
            </tr>
          </tbody>
        </table>
        <div>
          Die Audiowiedergabe im Browser hängt vom verwendeten Browser ab.
          <br />
          Dies Programm verwendet den Audio.play() befehl mit JavaScript.
          <br />
          <br />
          Microsoft Edge:
          <br />
          Mit Edge funktioniert die Wiedergabe solange die Seite im Vordergrund ist.
          <br />
          Einstellung: Websiteberechtigungen / Automatische Medienwiedergabe: Zulassen.
          <br />
          <br />
          Google Chrome:
          <br />
          Mit Chrome muß die Webseite mindestens einmal angekickt werden damit die Audiowiedergabe funktioniert.
          <br />
          <br />
          Firefox:
          <br />
          Mit Firefox funktioniert die Wiedergabe solange die Seite im Vordergrund ist.
          <br />
          Einstellung: Berechtigungen / Automatische Wiedergabe: Audio und Video erlauben
          <br />
          <br />
          Mobiele Browser können wieder anders reagieren.
          <br />
          Zukünftige Änderungen an den Browsern können dies Verhalten wieder verändern.
        </div>
      </div>
    );
  }
}

Basics.propTypes = {
  options: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
};

Basics.defaultProps = {
  options: [],
  websocketSend: () => {},
};

export default Basics;
