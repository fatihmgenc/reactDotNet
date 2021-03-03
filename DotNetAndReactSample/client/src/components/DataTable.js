import React, { Component } from "react";
import { Table, Button, Container, Col, Row } from "reactstrap";

class DataTable extends Component {
    
    render() {
        const items = this.props.items.map(item => {
            return (
                <tr key={item.StockCode}>
                    <th scope="row">{item.StockCode}</th>
                    <td>{item.StockName}</td>
                    <td>{item.BrandName}</td>
                    <td>{item.Price}</td>
                </tr>
            );
        });

        return (
                <div
                    style={{
                        maxHeight: "600px",
                        overflowY: "auto"
                    }}
                    >
                    <Table responsive hover>
                        <thead>
                            <tr>
                                <th>Stock Code</th>
                                <th>Stok Name</th>
                                <th>Brand</th>
                                <th>Price</th>
                            </tr>
                        </thead>
                        <tbody>{items}</tbody>
                    </Table>
                </div>
        );
    }
}

export default DataTable;
