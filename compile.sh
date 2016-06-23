#!/usr/bin/env bash

makeglossaries PFG-Santiago-Mazagatos
biber PFG-Santiago-Mazagatos
xelatex -shell-escape PFG-Santiago-Mazagatos
