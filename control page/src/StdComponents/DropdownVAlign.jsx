import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { MdVerticalAlignBottom, MdVerticalAlignCenter, MdVerticalAlignTop } from 'react-icons/md';

class DropdownVAlign extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleClick(e) {
    this.props.onClick(e);
  }

  calcAlignIcon(h) {
    if (h === 'flex-start') {
      return (<MdVerticalAlignTop />);
    }

    if (h === 'center') {
      return (<MdVerticalAlignCenter />);
    }

    if (h === 'flex-end') {
      return (<MdVerticalAlignBottom />);
    }

    return '';
  }

  render() {
    const aicon = this.calcAlignIcon(this.props.valign);
    return (
      <div className="">
        <div className="dropdown">
          <button
            className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 dropdown-toggle"
            type="button"
            data-placement="right"
            title="vertikale Ausrichtung"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            {aicon}
          </button>
          <div className="dropdown-menu p-0">

            <div className="d-flex flex-row">
              <button
                type="button"
                value="flex-start"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'flex-start')}
                title="oben"
              >
                <div className="d-flex flex-row align-items-center">
                  <MdVerticalAlignTop />
                  <div className="ml-1">oben</div>
                </div>
              </button>
            </div>
            <div className="d-flex flex-row">
              <button
                type="button"
                value="center"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'center')}
                title="zentriert"
              >
                <div className="d-flex flex-row align-items-center">
                  <MdVerticalAlignCenter />
                  <div className="ml-1">zentriert</div>
                </div>
              </button>
            </div>
            <div className="d-flex flex-row">
              <button
                type="button"
                value="flex-end"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'flex-end')}
                title="unten"
              >
                <div className="d-flex flex-row align-items-center">
                  <MdVerticalAlignBottom />
                  <div className="ml-1">unten</div>
                </div>
              </button>
            </div>

          </div>
        </div>
      </div>
    );
  }
}

DropdownVAlign.propTypes = {
  valign: PropTypes.string,
  onClick: PropTypes.func,
};

DropdownVAlign.defaultProps = {
  valign: '',
  onClick: () => {},
};

export default DropdownVAlign;
