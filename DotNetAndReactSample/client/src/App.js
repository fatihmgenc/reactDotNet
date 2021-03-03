import React, { Component } from "react";
import { Container, Row, Col, Button } from "reactstrap";
import DataTable from "./components/DataTable";
import "./index.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { TablePagination, TextField } from "@material-ui/core";
import axios from 'axios';

class App extends Component {
  constructor(props) {
    super(props);
    this.state = {
      items: [],
      page: 0,
      rowsPerPage: 10,
      filterStockCode: "",
      filterStockName: "",
      filterBrandName: "",
      filterPrice: "",
      totalRows: 11,
    }
    this.handleChange = this.handleChange.bind(this);
  };

  getItems() {
    let url = `https://localhost:5001/api/Stock/Get?page=${this.state.page +
      1}&countPerPage=${this.state.rowsPerPage}`;
    // let url = "https://jsonplaceholder.typicode.com/users";
    fetch(url)
      .then(res => res.json())
      .then(data => {
        this.setState({ items: data.Stocks, totalRows: data.StockCount });
      })
      .catch(err => console.log(err));
  }

  askPdf() {

    axios(`https://localhost:5001/api/PdfCreator?page=${this.state.page +
      1}&countPerPage=${this.state.rowsPerPage}`, {
      method: 'GET',
      responseType: 'blob' //Force to receive data in a Blob Format
    })
      .then(response => {
        //Create a Blob from the PDF Stream
        const file = new Blob(
          [response.data],
          { type: 'application/pdf' });
        //Build a URL from the file
        const fileURL = URL.createObjectURL(file);
        //Open the URL on new Window
        window.open(fileURL);
      })
      .catch(error => {
        console.log(error);
      });
  }


  updateState = item => {
    const itemIndex = this.state.items.findIndex(data => data.id === item.id);
    const newArray = [
      // destructure all items from beginning to the indexed item
      ...this.state.items.slice(0, itemIndex),
      // add the updated item to the array
      item,
      // add the rest of the items to the array from the index after the replaced item
      ...this.state.items.slice(itemIndex + 1)
    ];
    this.setState({ items: newArray });
  };

  deleteItemFromState = id => {
    const updatedItems = this.state.items.filter(item => item.id !== id);
    this.setState({ items: updatedItems });
  };

  componentDidMount() {
    this.getItems();
  }
  handleChangeRowsPerPage = event => {
    this.setState({ rowsPerPage: parseInt(event.target.value, 10) });
    this.setState({ page: this.state.page }, () => {
      this.getItems();
    });
  };
  handleChangePage = (event, newPage) => {
    this.setState({ page: newPage }, () => {
      this.getItems();
    });
  };

  handleChange(e) {
    this.setState({ [e.target.name]: e.target.value });
  }
  render() {
    return (
      <div>
        <Container className="App">
          <Row>

            <Col style={{ marginTop: "50px" }} xs="2">
              <TextField value={this.state.filterStockCode} onChange={this.handleChange} name="filterStockCode" label="StockCode" variant="filled" />
              <TextField value={this.state.filterStockName} onChange={this.handleChange} name="filterStockName" label="Stok Name" variant="filled" />
              <TextField value={this.state.filterBrandName} onChange={this.handleChange} name="filterBrandName" label="Brand" variant="filled" />
              <TextField value={this.state.filterPrice} onChange={this.handleChange} name="filterPrice" label="Price" variant="filled" />
              <Button style={{marginTop:"5px",display:"center"}} color="success" onClick={() => this.askPdf()}>Get this page as PDF</Button>
            </Col>

            <Col >
              <Row>

                <h1 style={{ margin: "20px 0" }}>Stock List</h1>
                
              </Row>

              <Row>
                <Col>
                  {this.state.items ? (
                    <DataTable
                      items={this.state.items.filter((item) =>
                        item.StockCode.toUpperCase().includes(this.state.filterStockCode.toUpperCase())
                        && item.StockName.toUpperCase().includes(this.state.filterStockName.toUpperCase())
                        && item.BrandName.toUpperCase().includes(this.state.filterBrandName.toUpperCase())
                        && item.Price.toUpperCase().includes(this.state.filterPrice.toUpperCase())
                      )}
                      updateState={this.updateState}
                      deleteItemFromState={this.deleteItemFromState}
                    />
                  ) : null}
                </Col>
              </Row>
              <TablePagination
                component="div"
                count={this.state.totalRows}
                page={this.state.page}
                onChangePage={this.handleChangePage}
                rowsPerPage={this.state.rowsPerPage}
                onChangeRowsPerPage={this.handleChangeRowsPerPage}
              />
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}

export default App;
