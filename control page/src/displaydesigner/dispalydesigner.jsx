/* eslint-disable react/no-access-state-in-setstate */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
// import $ from 'jquery';
import DisplayPageSets from './DisplayPageSets';
import DisplayPages from './DisplayPages';
import DisplayObjects from './DisplayObjects';
import ObjectProperties from './ObjectProperties';
import IFrame from './IFrame';

class DispalyDesigner extends Component {
  constructor(props) {
    super(props);
    this.state = {
      // designerurl: '',
      selectedPageSet: '',
      selectedObjectIDs: [],
      iFrameRefresh: false,
      activeObject: '',
    };
  }

  // after render
  componentDidMount() {
    this.PageSetSelection(this.props.PageSets[0]);
  }

  componentDidUpdate(prevProps, prevState) {
    // console.log('displaydesigner did update');
    if (prevProps.pages !== this.props.pages) {
      if (this.props.pages.length > 0) {
        this.pageSelection(this.props.pages[0]);
      }
    }

    if (prevProps.PageSets !== this.props.PageSets) {
      if (this.props.PageSets.length > 0) {
        this.PageSetSelection(this.props.PageSets[0]);
      }
    }

    if (prevProps.allDivs !== this.props.allDivs && prevState.activeObject !== '') {
      const activeObjectnew = this.props.allDivs.find((x) => x.ID === prevState.activeObject.ID);
      if (activeObjectnew !== prevState.activeObject) {
        this.markObjekt(activeObjectnew);
      }
    }
  }

  getDivs(page) {
    let p = page.ID;
    if (p === null) {
      p = this.props.page.ID;
    }
    this.props.websocketSend({
      Type: 'req',
      Command: 'Divs',
      PageSet: this.state.selectedPageSet.ID,
      Page: p,
    });
  }

  setMarkColor(MarkColor) {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'MarkColor',
      PageSet: this.state.selectedPageSet.ID,
      Page: this.props.page.ID,
      Value1: MarkColor,
    });
  }

  GridChange(grid) {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetPageGrid',
      PageSet: this.state.selectedPageSet.ID,
      Page: this.props.page.ID,
      Value1: grid,
    });
  }

  pageSelection(page) {
    this.props.websocketSend({
      Type: 'req', Command: 'Page', PageSet: this.state.selectedPageSet.ID, Page: page.ID,
    });
    this.refreshDivs(page);
  }

  pageStyleChange(styleProperty, styleValue) {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetPageStyle',
      PageSet: this.state.selectedPageSet.ID,
      Page: this.props.page.ID,
      Property: styleProperty,
      Value1: styleValue,
    });
  }

  PageSetSelection(profil) {
    this.props.websocketSend({ Type: 'req', Command: 'Pages', PageSet: profil.ID });
    this.setState({ selectedPageSet: profil });
  }

  objectSelection(ID, checked) {
    if (checked) {
      const so = this.state.selectedObjectIDs;
      so.push(ID);
      this.setState({ selectedObjectIDs: so });

      this.markObjekt(this.props.allDivs.find((x) => x.ID === ID));
    } else {
      const so = this.state.selectedObjectIDs;
      const pos = so.indexOf(ID);
      if (pos > -1) {
        so.splice(pos, 1);
        this.setState({ selectedObjectIDs: so });
        if (so.length > 0) {
          this.markObjekt(this.props.allDivs.find((x) => x.ID === so[0]));
        } else {
          this.unmarkObjekt();
        }
      }
    }
    // this.setState({ allDivs: all });
  }

  markObjekt(Objekt) {
    this.setState({ activeObject: Objekt });
  }

  unmarkObjekt() {
    this.setState({ activeObject: '' });
    this.setState({ selectedObjectIDs: [] });
  }

  refreshDivs(page) {
    this.unmarkObjekt();
    this.getDivs(page);
  }

  ObjektStyleChange(styleProperty, styleValue) {
    if (this.state.activeObject !== null) {
      const o = this.state.activeObject;
      const os = JSON.parse(o.style);
      os[styleProperty] = styleValue;
      o.style = JSON.stringify(os);
      this.setState({ activeObject: o });

      this.props.websocketSend({
        Type: 'bef',
        Command: 'setDivStyle',
        PageSet: this.state.selectedPageSet.ID,
        Page: this.props.page.ID,
        Divs: this.state.selectedObjectIDs,
        Property: styleProperty,
        Value1: styleValue,
      });
    }
  }

  ObjektAttributeChange(Attribute, AttributeValue) {
    if (this.state.activeObject !== null) {
      const o = this.state.activeObject;
      o[Attribute] = AttributeValue;
      this.setState({ activeObject: o });

      this.props.websocketSend({
        Type: 'bef',
        Command: 'setDivAttribute',
        PageSet: this.state.selectedPageSet.ID,
        Page: this.props.page.ID,
        Divs: this.state.selectedObjectIDs,
        Property: Attribute,
        Value1: AttributeValue,
      });
    }
  }

  render() {
    return (
      <div className="" style={{ maxHeight: '100vh' }}>
        <div className="text-center bg-fsc text-light p-1 mb-1">
          Anzeige-Editor
        </div>
        <div className="">
          <div className="d-flex border border-dark">
            <div className="d-flex flex-column border border-dark">
              <DisplayPageSets
                PageSets={this.props.PageSets}
                websocketSend={this.props.websocketSend.bind(this)}
                websocketSendRaw={this.props.websocketSendRaw.bind(this)}
                profil={this.state.selectedPageSet}
                PageSetSelection={this.PageSetSelection.bind(this)}
              />

              <DisplayPages
                pages={this.props.pages}
                profil={this.state.selectedPageSet}
                page={this.props.page}
                picList={this.props.picList}

                websocketSend={this.props.websocketSend.bind(this)}
                pageSelection={this.pageSelection.bind(this)}
                onPageStyleChange={this.pageStyleChange.bind(this)}
                markColor={this.setMarkColor.bind(this)}
                GridChange={this.GridChange.bind(this)}
              />

              <DisplayObjects
                allDivs={this.props.allDivs}
                websocketSend={this.props.websocketSend.bind(this)}
                profil={this.state.selectedPageSet}
                page={this.props.page}
                refreshDivs={this.refreshDivs.bind(this)}
                objectSelection={this.objectSelection.bind(this)}
                selectedObjectIDs={this.state.selectedObjectIDs}
              />

            </div>

            <div className="d-flex flex-column flex-grow-1 border border-dark">

              <ObjectProperties
                objekt={this.state.activeObject}
                picVariables={this.props.picVariables}
                textVariables={this.props.textVariables}
                tableVariables={this.props.tableVariables}
                picList={this.props.picList}
                fontList={this.props.fontList}

                ObjektStyleChange={this.ObjektStyleChange.bind(this)}
                ObjektAttributeChange={this.ObjektAttributeChange.bind(this)}

                refreshDivs={this.refreshDivs.bind(this)}
              />

              <div className="d-flex flex-row border border-dark p-2 bg-secondary">
                <IFrame
                  wsp={this.props.wsp}
                  profil={this.state.selectedPageSet.ID}
                  page={this.props.page.ID}
                  objektid={this.state.activeObject ? this.state.activeObject.ID : ''}
                  refresh={this.state.iFrameRefresh}
                />
              </div>

            </div>
          </div>
        </div>
      </div>
    );
  }
}

DispalyDesigner.propTypes = {
  wsp: PropTypes.string.isRequired,
  picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  textVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  tableVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  pages: PropTypes.arrayOf(PropTypes.object).isRequired,
  PageSets: PropTypes.arrayOf(PropTypes.object).isRequired,
  picList: PropTypes.arrayOf(PropTypes.object).isRequired,
  fontList: PropTypes.arrayOf(PropTypes.object).isRequired,
  page: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func.isRequired,
  allDivs: PropTypes.arrayOf(PropTypes.object),
  websocketSendRaw: PropTypes.func.isRequired,
};

DispalyDesigner.defaultProps = {
  allDivs: [],
  page: '',
};

export default DispalyDesigner;
