import json
import os
from flask import Flask, flash, request, redirect, url_for,jsonify, send_from_directory
from werkzeug.utils import secure_filename
import librosa
import numpy as np
import midiutil
import io
import pretty_midi
import json
import convert3

ALLOWED_EXTENSIONS = {'txt', 'pdf', 'png', 'jpg', 'jpeg', 'gif', 'midi', 'mid', 'wav'}
secret_key = os.urandom(24).hex() 
app = Flask(__name__)
app.secret_key = secret_key

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
UPLOAD_FOLDER = os.path.join(BASE_DIR, 'uploads')
DOWNLOAD_FOLDER = os.path.join(BASE_DIR, 'output')
app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER
app.config['DOWNLOAD_FOLDER'] = DOWNLOAD_FOLDER





os.makedirs(UPLOAD_FOLDER, exist_ok=True)
os.makedirs(DOWNLOAD_FOLDER, exist_ok=True)






def allowed_file(filename):
    return '.' in filename and \
           filename.rsplit('.', 1)[1].lower() in ALLOWED_EXTENSIONS
def audio_splicing(filename):
    midi_bytes = convert3.run_audio(filename)
    


  #  output_path = os.path.join(
  #  app.config['DOWNLOAD_FOLDER'],
  #  os.path.basename(filename) + "_output.json"
#)

#    with open(output_path) as f:
#        json.dump(midi_json, f, indent = 4)
    return jsonify(midi_bytes)
@app.route('/employees', methods=['GET'])

def get_employees():
    return jsonify([{'id':1}])



@app.route('/', methods=['POST'])
def upload_file():
    
    if request.method == 'POST':
        # check if the post request has the file part
        
        if 'file' not in request.files:
            print('nofilep part')
            flash('No file part')
            return redirect(request.url)
        file = request.files['file']
        print(file.filename)
        # If the user does not select a file, the browser submits an
        # empty file without a filename.
        if file.filename == '':
            flash('No selected file')
            print("No selected file")
            return redirect(request.url)
        if file and allowed_file(file.filename):
            print(request.files['file'])
            print(request.form)
            filename = secure_filename(file.filename)



            filename = secure_filename(file.filename)
            save_path = os.path.join(app.config['UPLOAD_FOLDER'], filename)

            file.save(save_path)  # Save first!

           # audio_splicing(save_path)  # Then process
#            save_path = (os.path.join(app.config['UPLOAD_FOLDER'],filename))
#            file = audio_splicing(save_path)
#            file.save(os.path.join(app.config['UPLOAD_FOLDER'], filename))
            output_json = audio_splicing(save_path)
            #return send_from_directory(
            #    app.config['DOWNLOAD_FOLDER'],
            #    os.path.basename(output_path),
            #    as_attachment=True
            #)
            #return send_from_directory(app.config["UPLOAD_FOLDER"], filename)
            return output_json
        print('fail')
    return "failed"

if __name__ == '__main__':
    app.run(port=5000)
