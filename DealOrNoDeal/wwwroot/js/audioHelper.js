window.audioHelper = {
    playAudio: function (audioSelector) {
        const audio = document.querySelector(audioSelector);
        if (audio) {
            return audio.play();
        }
        return Promise.reject("Audio element not found");
    },
    
    getAudioDuration: function (audioSelector) {
        return new Promise((resolve, reject) => {
            const audio = document.querySelector(audioSelector);
            if (!audio) {
                reject("Audio element not found");
                return;
            }
            
            if (audio.duration && !isNaN(audio.duration)) {
                resolve(audio.duration * 1000); // Convert to milliseconds
            } else {
                audio.addEventListener('loadedmetadata', () => {
                    resolve(audio.duration * 1000);
                }, { once: true });
            }
        });
    },
    
    onAudioEnded: function (audioSelector, dotNetHelper, methodName) {
        const audio = document.querySelector(audioSelector);
        if (audio) {
            audio.addEventListener('ended', () => {
                dotNetHelper.invokeMethodAsync(methodName);
            }, { once: true });
        }
    }
};

