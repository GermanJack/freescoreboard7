/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DlgPicSelection from '../StdComponents/DlgPicSelection';

class TeamDetail extends Component {
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
        Type: 'bef',
        Command: 'TeamChange',
        Team: this.props.team.ID,
        Property: feld,
        Value1: e.target.value,
      });
    }
  }

  handleBildWahl(nr, e) {
    if (this.props.team === '') {
      return;
    }
    this.props.websocketSend({
      Bereich: 'MM',
      Type: 'bef',
      Command: 'TeamChange',
      Team: this.props.team.ID,
      Property: `Bild${nr}`,
      Value1: e,
    });
  }

  render() {
    const b1 = this.props.team && this.props.team.Bild1 ? <img src={`./../../pictures/${this.props.team.Bild1}`} alt="kein Bild gewählt" style={{ maxHeight: '10rem', maxWidth: '10rem', align: 'middle' }} /> : 'kein Bild gewählt';
    const b2 = this.props.team && this.props.team.Bild1 ? <img src={`./../../pictures/${this.props.team.Bild2}`} alt="kein Bild gewählt" style={{ maxHeight: '10rem', maxWidth: '10rem', align: 'middle' }} /> : 'kein Bild gewählt';
    const manName = this.props.team ? this.props.team.Name : '';
    const manKName = this.props.team ? this.props.team.Kurzname : '';

    return (
      <div className="d-flex flex-column flex-grow-1 border border-fsc mh-25">
        <div className="d-flex flex-row border border-fsc bg-secondary text-light pl-2">
          Mannschaftendetails:
          <div className="pl-2">
            <b>
              {manName}
            </b>
          </div>
        </div>

        <div className="d-flex flex-row border border-fsc">

          <form className="">

            <div className="form-group row m-1">
              <label htmlFor="Name" className="col-sm-4 col-form-label">Name</label>
              <div className="col-sm-8">
                <input
                  type="text"
                  className="form-control"
                  data-toggle="tooltip"
                  title="Name"
                  spellCheck="false"
                  value={manName}
                  onChange={this.handleTextChange.bind(this, 'Name')}
                  id="Name"
                />
              </div>
            </div>

            <div className="form-group row m-1">
              <label htmlFor="Kurzname" className="col-sm-4 col-form-label">Kurzname</label>
              <div className="col-sm-8">
                <input
                  type="text"
                  className="form-control"
                  data-toggle="tooltip"
                  title="Kurzname"
                  spellCheck="false"
                  value={manKName}
                  onChange={this.handleTextChange.bind(this, 'Kurzname')}
                  id="Kurzname"
                />
              </div>
            </div>

            <div className="row ml-3">
              <div className="card bg-light mb-2 mr-3" style={{ maxWidth: '18rem' }}>
                <div className="card-header p-1">
                  <div className="d-flex flexrow">
                    <div className="card-title mr-1">Bild1:</div>
                    <DlgPicSelection
                      label1="Bildwahl:"
                      data-toggle="tooltip"
                      data-placement="right"
                      toolTip="Bild1"
                      class="btn btn-outline-secondary btn-icon btn-sm"
                      modalID="Modal-H1"
                      values={this.props.picList}
                      selection={this.handleBildWahl.bind(this, '1')}
                    />
                  </div>
                </div>
                <div className="card-body p-2 align-middle">
                  {b1}
                </div>
              </div>

              <div className="card bg-light mb-2  mr-3" style={{ maxWidth: '18rem' }}>
                <div className="card-header p-1">
                  <div className="d-flex flexrow">
                    <div className="card-title mr-1">Bild2:</div>
                    <DlgPicSelection
                      label1="Bildwahl:"
                      data-toggle="tooltip"
                      data-placement="right"
                      toolTip="Bild2"
                      class="btn btn-outline-secondary btn-icon btn-sm"
                      modalID="Modal-H2"
                      values={this.props.picList}
                      selection={this.handleBildWahl.bind(this, '2')}
                    />
                  </div>
                </div>
                <div className="card-body p-2 align-middle">
                  {b2}
                </div>
              </div>
            </div>

          </form>

        </div>
      </div>
    );
  }
}

TeamDetail.propTypes = {
  team: PropTypes.oneOfType(PropTypes.object),
  picList: PropTypes.oneOfType(PropTypes.string),
  websocketSend: PropTypes.func,
};

TeamDetail.defaultProps = {
  team: null,
  picList: null,
  websocketSend: () => {},
};

export default TeamDetail;
