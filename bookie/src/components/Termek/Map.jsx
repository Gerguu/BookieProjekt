import { GoogleMap, LoadScript, Marker } from "@react-google-maps/api";
import { useEffect, useMemo, useRef } from "react";
import "./Termek.css"

function MyMap(lati){
    const mapRef = useRef(null);
    useEffect(() => {
        if (mapRef.current) {
            mapRef.current.panTo(defaultCenter); 
        }
    }, []);
    const markerPosition = useMemo(() => ({
        lat: lati.lati,
        lng: lati.longi,
      }), []);
    const defaultCenter = {
        lat: 46.708704399274865, lng: 20.18879505023016,
      };
    return (
        <LoadScript googleMapsApiKey="AIzaSyBAz3NgvETAjFMzP88Q86sJRfLyJj0P0_g">
            <GoogleMap id="map" center={defaultCenter}
                zoom={7.3}
                onLoad={(map) => (mapRef.current = map)} 
                options={{
                    mapTypeControl: false,      
                    fullscreenControl: false,   
                    streetViewControl: false,   
                    zoomControl: false,         
                  }}
                mapContainerStyle={{ height: "500px", width: "55rem"}}>
                <Marker position={markerPosition} />
            </GoogleMap>
        </LoadScript>
    );
};

export default MyMap;