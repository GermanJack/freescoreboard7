import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { AiOutlinePicture } from 'react-icons/ai';
import Dropdown from './Dropdown';

class DlgPicSelection extends Component {
  constructor(props) {
    super(props);
    this.state = {
      folder: 'Allgemein',
    };
  }

  handleclick(pic) {
    if (pic === '[kein Bild]') {
      this.props.selection('');
    } else {
      this.props.selection(pic);
    }
  }

  filter(e) {
    this.setState({ folder: e });
  }

  render() {
    let pl = [];
    let pl2 = [];
    if (this.props.values.length > 0) {
      pl = this.props.values;
      for (let i = 1; i < pl.length; i += 1) { // 0:[kein Bild] daher ab 1
        const x = pl[i].split('/');
        if (x[0] === this.state.folder) {
          pl2.push(x[1]);
        }
      }
    }
    pl2.unshift('[kein Bild]');

    let items = [];
    if (this.props.values) {
      items = pl2.map((i) => { // eslint-disable-line arrow-body-style
        return (
          <button
            type="button"
            className="m1 btn btn-outline-dark"
            key={i}
            data-dismiss="modal"
            onClick={this.handleclick.bind(this, `${this.state.folder}/${i}`)}
          >
            <div className="m-1 p-1 d-flex flex-row align-items-center border border-dark">
              <img src={`./../../../pictures/${this.state.folder}/${i}`} alt="" style={{ maxHeight: '10rem', maxWidth: '10rem', align: 'middle' }} />
              <div className="pl-1">{i}</div>
            </div>
          </button>
        );
      });
    }
    let cn = 'btn btn-primary btn-icon border-dark p-0 mr-1';
    if (this.props.class !== '') {
      cn = this.props.class;
    }
    let image = (<AiOutlinePicture size="1.5em" />);
    if (this.props.reacticon !== '') {
      image = this.props.reacticon;
    }
    return (
      <div>
        <button
          type="button"
          className={cn}
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
        >
          {image}
        </button>

        <div className="modal fade" id={this.props.modalID} tabIndex="-1" role="dialog" aria-labelledby={this.props.modalID} aria-hidden="true">
          <div className="modal-dialog modal-dialog-centered" role="document">
            <div className="modal-content">

              <div className="modal-header">
                <h5 className="modal-title text-dark">{this.props.label1}</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>

              <div className="modal-body p-1">

                <div className="d-flex flex-row ml-1 mb-1">
                  <div className="mr-1">Pfad:</div>
                  <Dropdown
                    values={['Allgemein', 'Mannschaften', 'Slideshow']}
                    value={this.state.folder}
                    wahl={this.filter.bind(this)}
                  />
                </div>

                <div className="d-flex flex-column" style={{ overflow: 'scroll', height: '50vh' }}>
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

DlgPicSelection.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  toolTip: PropTypes.string,
  values: PropTypes.arrayOf(PropTypes.string),
  selection: PropTypes.func.isRequired,
  reacticon: PropTypes.string,
  class: PropTypes.string,
};

DlgPicSelection.defaultProps = {
  modalID: 'filedlg',
  label1: 'Bildauswahl',
  toolTip: '',
  values: [],
  reacticon: '',
  class: '',
};

export default DlgPicSelection;
