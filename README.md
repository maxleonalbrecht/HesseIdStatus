# HesseIdStatus
Simplified api endpoint to check if a requested Id (Personalausweis or Reisepass) is ready to pickup

API Endpoint running at: https://hesseidstatus.azurewebsites.net/api/HesseIdStatus

## Parameters

| Name         | Description                                                                                 | Example                        |
|--------------|---------------------------------------------------------------------------------------------|--------------------------------|
| idIdentifier | Identifier of the new document. This Identifier is issued to you, at the moment of request. | L9XY1L1234                     |
| idType       | Type of the id                                                                              | rbPersonalausweis, rbReisepass |
| issuingCity  | Hesse cities that are using eKom21                                                          | frankfurt, giessen, offenbach  |

## Example

### Request

Postwoman Example: [Link](https://postwoman.io/?method=GET&url=https://hesseidstatus.azurewebsites.net&path=/api/HesseIdStatus?idIdentifierL2872=%253Cid%253E&idType=rbPersonalausweis&issuingCity=frankfurt&params=%5B%7B%22key%22:%22idIdentifierL2872%22,%22value%22:%22%253Cid%253E%22,%22type%22:%22query%22%7D,%7B%22key%22:%22idType%22,%22value%22:%22rbPersonalausweis%22,%22type%22:%22query%22%7D,%7B%22key%22:%22issuingCity%22,%22value%22:%22frankfurt%22,%22type%22:%22query%22%7D%5D)

### Response

```json
{
  "idIdentifier": "L9XY1L1234",
  "idType": "rbPersonalausweis",
  "issuingCity": "frankfurt",
  "idStatus": {
    "readyForPickup": false,
    "pickedUp": false,
    "inProduction": true
  }
}

```
