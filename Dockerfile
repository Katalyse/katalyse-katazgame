FROM caddy
COPY /KatalyseProject/BuildProject/Build_Final/ /usr/share/caddy/
RUN rm /usr/share/caddy/Build_Final.zip