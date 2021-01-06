import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GrNewWindow } from 'react-icons/gr';
import Checkbox from './Checkbox';

class DlgFensterwahl extends Component {
  constructor(props) {
    super(props);
    this.state = {
      modalVisible: false,
    };

    this.tooglemodal = this.tooglemodal.bind(this);
  }

  tooglemodal() {
    this.setState((prevState) => ({
      modalVisible: !prevState.modalVisible,
    }));
  }

  handleclick(e) {
    const fen = this.props.werte;
    const i = fen.findIndex((x) => x.Objekt === e);

    if (fen[i].Sichtbar === 1) {
      fen[i].Sichtbar = 0;
    } else {
      fen[i].Sichtbar = 1;
    }

    const fjson = JSON.stringify(fen[i]);
    this.props.websocketSend({ Type: 'bef', Command: 'SaveWebKontrol', Value1: fjson });
  }

  render() {
    this.props.werte.sort((a, b) => ((a.Title > b.Title) ? 1 : ((b.Title > a.Title) ? -1 : 0)));
    const items = this.props.werte.map((i) => { // eslint-disable-line arrow-body-style
      return (
        <Checkbox
          key={i.Objekt}
          label={i.Title}
          checked={i.Sichtbar}
          handleCheckboxChange={this.handleclick.bind(this, i.Objekt)}
        />
      );
    });

    const modalID = 'Fensterwahl';

    return (
      <div>
        <div
          role="button"
          className="p-0 ml-1"
          style={{ backgroundColor: 'transparent' }}
          data-placement="right"
          title="Fensterwahl"
          label="Fensterwahl"
          data-toggle="modal"
          data-target={`#${modalID}`}
        >
          <GrNewWindow size="0.8em" />
        </div>

        <div className="modal fade" id={modalID} tabIndex="-1" role="dialog" aria-labelledby={modalID} aria-hidden="true">
          <div className="modal-dialog modal-dialog-centered" role="document">
            <div className="modal-content">

              <div className="modal-header">
                <h5 className="modal-title text-dark">{this.props.label1}</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>

              <div className="modal-body p-1">
                <div className="" style={{ overflow: 'scroll', height: '50vh' }}>
                  {items}
                </div>
              </div>

              <div className="modal-footer">
                <div className="text-dark">{this.state.meldung}</div>
              </div>

            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgFensterwahl.propTypes = {
  werte: PropTypes.arrayOf(PropTypes.object),
  label1: PropTypes.string,
  websocketSend: PropTypes.func,
};

DlgFensterwahl.defaultProps = {
  werte: '#000000',
  label1: '',
  websocketSend: () => {},
};

export default DlgFensterwahl;
