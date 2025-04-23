import requests

host = "http://localhost:5044"
animalId = "33333333-3333-3333-3333-333333333333"

def main():
    observationId = CreateObservation()
    recordId = CreateObservationRecord(observationId)
    observations = ListObservations()
    # find the observation which contains the recordId
    for observation in observations:
        if recordId in observation.get("recordIds", []):
            print(f"Observation ID: {observation.get('id')} contains the record ID: {recordId}")
            break
    updateRecord(observationId, recordId)
    updateObservation(observationId)
    deleteRecord(observationId, recordId)
    deleteObservation(observationId)

def CreateObservation():
    url = f"{host}/api/observations"
    headers = {
        "Content-Type": "application/json"
    }
    body = {
        "name": "Test Observation",
        "description": "This is a test observation"
    }
    response = requests.post(url, json=body, headers=headers)
    if response.status_code == 201:
        print(f"Observation created successfully. ID: {response.json().get('id')}")
        return response.json().get("id")
    else:
        raise Exception(f"Failed to create observation. Status code: {response.status_code}. Content: {response.content}")

def CreateObservationRecord(observationId):
    url = f"{host}/api/observations/{observationId}/records"
    headers = {
        "Content-Type": "application/json"
    }
    body = {
        "AnimalId": animalId,
        "Location": {
            "Latitude": 45.0,
            "Longitude": 25.0
        },
        "Timestamp": "2023-10-01T12:00:00Z",
        "Comment": "This is a test record"
    }
    response = requests.post(url, json=body, headers=headers)
    if response.status_code == 201:
        print(f"Record created successfully. ID: {response.json().get('id')}")
        return response.json().get("id")
    else:
        raise Exception(f"Failed to create record. Status code: {response.status_code}. Content: {response.content}")

def ListObservations():
    url = f"{host}/api/observations"
    headers = {
        "Content-Type": "application/json"
    }
    response = requests.get(url, headers=headers)
    if response.status_code == 200:
        print("Observations retrieved successfully.")
        for observation in response.json():
            print(f"ID: {observation.get('id')}, Title: {observation.get('title')}")
        return response.json()
    else:
        raise Exception(f"Failed to retrieve observations. Status code: {response.status_code}. Content: {response.content}")

def updateObservation(observationId):
    url = f"{host}/api/observations/{observationId}"
    headers = {
        "Content-Type": "application/json"
    }
    body = {
        "title": "Updated Observation",
        "description": "This is an updated test observation"
    }
    response = requests.patch(url, json=body, headers=headers)
    if response.status_code == 204:
        print(f"Observation ({observationId}) updated successfully.")
    else:
        raise Exception(f"Failed to update observation ({observationId}). Status code: {response.status_code}. Content: {response.content}")

def updateRecord(observationId, recordId):
    url = f"{host}/api/observations/{observationId}/records/{recordId}"
    headers = {
        "Content-Type": "application/json"
    }
    body = {
        "location": {
            "Latitude": 40.0,
            "Longitude": 21.0
        },
        "timestamp": "2023-10-02T12:00:00Z"
    }
    response = requests.patch(url, json=body, headers=headers)
    if response.status_code == 204:
        print(f"Record ({recordId}) updated successfully.")
    else:
        raise Exception(f"Failed to update record (ObservationId: {observationId}. RecordId {recordId}). Status code: {response.status_code}. Content: {response.content}")

def deleteObservation(observationId):
    url = f"{host}/api/observations/{observationId}"
    headers = {
        "Content-Type": "application/json"
    }
    response = requests.delete(url, headers=headers)
    if response.status_code == 204:
        print(f"Observation {observationId} deleted successfully.")
    else:
        raise Exception(f"Failed to delete observation ({observationId}). Status code: {response.status_code}. Content: {response.content}")

def deleteRecord(observationId, recordId):
    url = f"{host}/api/observations/{observationId}/records/{recordId}"
    headers = {
        "Content-Type": "application/json"
    }
    response = requests.delete(url, headers=headers)
    if response.status_code == 204:
        print(f"Record {recordId} deleted successfully.")
    else:
        raise Exception(f"Failed to delete record ({recordId}). Status code: {response.status_code}. Content: {response.content}")

if __name__ == "__main__":
    try:
        main()
    except Exception as e:
        print(f"An error occurred: {e}")

