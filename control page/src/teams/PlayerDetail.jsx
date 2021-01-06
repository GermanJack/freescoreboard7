/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DlgPicSelection from '../StdComponents/DlgPicSelection';

class PlayerDetail extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleTextChange(feld, e) {
    if (this.props.team === '') {
      return;
    }
    const text = e.target.value;
    const re = new RegExp('[^A-Za-z0-9_ßäÄüÜöÖ-]');
    const notok = re.exec(text);
    if (!notok) {
      this.props.websocketSend({
        Domain: 'MM',
        Type: 'bef',
        Command: 'PlayerChange',
        Team: this.props.team.ID,
        Player: this.props.player.ID,
        Property: feld,
        Value1: text,
      });
    }
  }

  handleBildWahl(e) {
    if (this.props.team === '') {
      return;
    }
    this.props.websocketSend({
      Domain: 'MM',
      Type: 'bef',
      Command: 'PlayerChange',
      Team: this.props.team.ID,
      Player: this.props.player.ID,
      Property: 'Bild',
      Value1: e,
    });
  }

  render() {
    const bild = this.props.player && this.props.player.Bild ? <img src={`./../../pictures/${this.props.player.Bild}`} alt="kein Bild gewählt" style={{ maxHeight: '10rem', maxWidth: '10rem', align: 'middle' }} /> : 'kein Bild gewählt';
    const NName = this.props.player && this.props.player.Nachname ? this.props.player.Nachname : '';
    const VName = this.props.player && this.props.player.Vorname ? this.props.player.Vorname : '';
    const SName = this.props.player && this.props.player.NickName ? this.props.player.NickName : '';
    const sid = this.props.player && this.props.player.SID ? this.props.player.SID : '';
    const disabled = !this.props.player;

    return (
      <div className="d-flex flex-column flex-grow-1 border border-fsc mh-25">
        <div className="d-flex flex-row border border-fsc bg-secondary text-light pl-2">
          Spielerdetails:
          <div className="pl-2">
            <b>
              {`${NName}, ${VName}`}
            </b>
          </div>
        </div>

        <div className="d-flex border border-fsc">
          <div className="">
            <div className="form-group row m-1">
              <label htmlFor="Nachname" className="col-sm-4 col-form-label">Nachname</label>
              <div className="col-sm-8">
                <input
                  type="Text"
                  className="form-control"
                  id="Nachname"
                  value={NName}
                  spellCheck="false"
                  disabled={disabled}
                  onChange={this.handleTextChange.bind(this, 'Nachname')}
                />
              </div>
            </div>
            <div className="form-group row m-1">
              <label htmlFor="Vorname" className="col-sm-4 col-form-label">Vorname</label>
              <div className="col-sm-8">
                <input
                  type="Text"
                  className="form-control"
                  id="Vorname"
                  value={VName}
                  spellCheck="false"
                  disabled={disabled}
                  onChange={this.handleTextChange.bind(this, 'Vorname')}
                />
              </div>
            </div>
            <div className="form-group row m-1">
              <label htmlFor="Spitzname" className="col-sm-4 col-form-label">Spitzname</label>
              <div className="col-sm-8">
                <input
                  type="Text"
                  className="form-control"
                  id="Spitzname"
                  value={SName}
                  spellCheck="false"
                  disabled={disabled}
                  onChange={this.handleTextChange.bind(this, 'NickName')}
                />
              </div>
            </div>
            <div className="form-group row m-1">
              <label htmlFor="ID-Nummer" className="col-sm-4 col-form-label">ID-Nummer</label>
              <div className="col-sm-8">
                <input
                  type="Text"
                  className="form-control"
                  id="ID-Nummer"
                  value={sid}
                  spellCheck="false"
                  disabled={disabled}
                  onChange={this.handleTextChange.bind(this, 'SID')}
                />
              </div>
            </div>
          </div>

          <div className="d-flex flex-col">
            <div className="card bg-light mb-2 mr-3" style={{ maxWidth: '18rem' }}>
              <div className="card-header p-1">
                <div className="d-flex flexrow">
                  <div className="card-title mr-1">Spielerbild:</div>
                  <DlgPicSelection
                    label1="Bildwahl:"
                    class="btn btn-outline-secondary btn-icon btn-sm"
                    data-toggle="tooltip"
                    data-placement="right"
                    toolTip="Bild"
                    modalID="Modal-H4"
                    values={this.props.picList}
                    selection={this.handleBildWahl.bind(this)}
                    disabled={disabled}
                  />
                </div>
              </div>
              <div className="card-body p-2 align-middle">
                {bild}
              </div>
            </div>

          </div>
        </div>
      </div>
    );
  }
}

PlayerDetail.propTypes = {
  team: PropTypes.oneOfType(PropTypes.object),
  player: PropTypes.oneOfType(PropTypes.object),
  picList: PropTypes.oneOfType(PropTypes.string),
  websocketSend: PropTypes.func,
};

PlayerDetail.defaultProps = {
  team: null,
  player: null,
  picList: null,
  websocketSend: () => {},
};

export default PlayerDetail;
