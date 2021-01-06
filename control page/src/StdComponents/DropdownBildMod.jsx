import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { BsGridFill, BsArrowsFullscreen, BsAspectRatio } from 'react-icons/bs';

class DropdownBildMod extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleClick(e) {
    this.props.onChange(e);
  }

  calcIcon(h) {
    let ret = '';
    if (h === '100% 100%') {
      ret = (<BsArrowsFullscreen />);
    } else if (h === 'contain') {
      ret = (<BsAspectRatio />);
    } else if (h === 'auto') {
      ret = (<BsGridFill />);
    }

    return ret;
  }

  render() {
    const aicon = this.calcIcon(this.props.value);
    return (
      <div className="">
        <div className="dropdown">
          <button
            className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 mr-1 dropdown-toggle"
            type="button"
            data-placement="right"
            title="Bild Modus"
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
                value="contain"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'contain')}
                title="proportional"
              >
                <div className="d-flex flex-row align-items-center">
                  <BsAspectRatio />
                  <div className="ml-1">proportional</div>
                </div>
              </button>
            </div>
            <div className="d-flex flex-row">
              <button
                type="button"
                value="100% 100%"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, '100% 100%')}
                title="gestreckt"
              >
                <div className="d-flex flex-row align-items-center">
                  <BsArrowsFullscreen />
                  <div className="ml-1">gestreckt</div>
                </div>
              </button>
            </div>
            <div className="d-flex flex-row">
              <button
                value="repeat"
                type="button"
                className="btn btn-outline-secondfary p-0 pl-1 pr-1"
                onClick={this.handleClick.bind(this, 'repeat')}
                title="repeat"
              >
                <div className="d-flex flex-row align-items-center">
                  <BsGridFill />
                  <div className="ml-1">wiederholt</div>
                </div>
              </button>
            </div>

          </div>
        </div>
      </div>
    );
  }
}

DropdownBildMod.propTypes = {
  value: PropTypes.string,
  onChange: PropTypes.func,
};

DropdownBildMod.defaultProps = {
  value: '',
  onChange: () => {},
};

export default DropdownBildMod;
